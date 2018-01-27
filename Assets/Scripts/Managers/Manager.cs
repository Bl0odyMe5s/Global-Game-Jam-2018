using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
	private List<PlayerObject> playerObjects;
	private GameStates state;
    private Ball ball;
    public static Manager manager;
	
	void Awake () {
		DontDestroyOnLoad(gameObject);

        manager = this;
		
		InitializeGame();
	}

	void InitializeGame()
	{
		playerObjects = new List<PlayerObject>{new PlayerObject("Player 1"), new PlayerObject("Player 2")};
		state = GameStates.Playing;
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

    public Ball Ball
    {
        get { return ball; }
        set { ball = value; }
    }
}
