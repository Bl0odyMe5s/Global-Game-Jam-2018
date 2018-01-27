using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class Manager : MonoBehaviour
{
    [SerializeField] private GameObject ball;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject soundWave;

    [SerializeField] int resetDelay;

    private GameStates state;
    public List<Vector3> spawnPoints;
    public static Manager manager;
    public KeyCode[] keyCodes;
    public List<GameObject> PlayerObjects { get; set; }
    public GameObject Ball { get; set; }
    public Ball BallScript { get; set; }

    [SerializeField]
    private List<int> playerScores;
	
	private void Awake () {
		DontDestroyOnLoad(gameObject);

        manager = this;
		PlayerObjects = new List<GameObject>();
        playerScores = new List<int> { 0,0 };
        spawnPoints = new List<Vector3>();
		
		InitializeGame();
	}

    public void SpawnLevel1()
    {
        SceneManager.LoadScene("GameScene");
        state = GameStates.Initializing;
        StartCoroutine(PopulateLevel1());
    }

    private IEnumerator PopulateLevel1()
    {
        yield return new WaitUntil(() => SceneManager.GetActiveScene().name == "GameScene");

        if (PlayerObjects.Count > 0)
        {
            PlayerObjects = new List<GameObject>();
            spawnPoints = new List<Vector3>();
        }

        GameObject player1 = GameObject.Instantiate(player);
        GameObject player2 = GameObject.Instantiate(player);

        player1.GetComponent<PlayerMechanics>().PlayerType = PlayerMechanics.PLAYER_ONE;
        player2.GetComponent<PlayerMechanics>().PlayerType = PlayerMechanics.PLAYER_TWO;
  
        PlayerObjects.Add(player1);
        PlayerObjects.Add(player2);

        PlayerObjects[0].GetComponent<PlayerMechanics>().id = 0;
        PlayerObjects[1].GetComponent<PlayerMechanics>().id = 1;
        PlayerObjects[0].GetComponent<PlayerMechanics>().CustomStart();
        PlayerObjects[1].GetComponent<PlayerMechanics>().CustomStart();

        Ball = GameObject.Find("Ball");
        int random = (int)Mathf.Round(Random.Range(0, 2));
        spawnPoints.Add(GameObject.Find("Ball Spawn 1").transform.position);
        spawnPoints.Add(GameObject.Find("Ball Spawn 2").transform.position);
        Ball.transform.position = spawnPoints[random];
        BallScript.PlayerY = player.transform.position.y;

        state = GameStates.Playing;
    }

    private void InitializeGame()
	{
		state = GameStates.Playing;
		SceneManager.LoadScene("MenuScene");
	}

    public List<int> PlayerScores
    {
        get { return playerScores; }
        set
        {
            playerScores = value;
        }
    }

    public int ResetDelay
    {
        get { return resetDelay; }
    }

	public GameStates State
	{
		get { return state; }
		set { state = value; }
	}

    public GameObject SoundWave
    {
        get { return soundWave; }
    }
}
