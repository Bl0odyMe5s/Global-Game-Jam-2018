using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
	private List<PlayerObject> playerObjects;
	private GameObject _ball;
	private GameStates state;
	
	private void Awake () {
		DontDestroyOnLoad(gameObject);
		
		PlayerObjects = new List<PlayerObject>();
		
		InitializeGame();
	}

	private void InitializeGame()
	{
		state = GameStates.Playing;
		SceneManager.LoadScene("GameScene");
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
}
