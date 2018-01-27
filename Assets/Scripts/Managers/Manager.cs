using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
	private List<PlayerObject> playerObjects;
	private GameObject _ball;
	private GameStates state;
    private Ball ball;
    public static Manager manager;
	
	private void Awake () {
		DontDestroyOnLoad(gameObject);

        manager = this;
		
		PlayerObjects = new List<PlayerObject>();
		
		InitializeGame();
	}

	private void InitializeGame()
	{
		state = GameStates.Playing;
		SceneManager.LoadScene("MenuScene");
	}

	public List<PlayerObject> PlayerObjects
	{
		get { return playerObjects; }
		set { playerObjects = value; }
	}

	public GameStates State
	{
		get { return state; }
		set { state = value; }
	}

	public GameObject Ball
	{
		get { return _ball; }
		set { _ball = value; }
	}

    public Ball BallComponent
    {
        get { return _ball.GetComponent<Ball>(); }
    }
}
