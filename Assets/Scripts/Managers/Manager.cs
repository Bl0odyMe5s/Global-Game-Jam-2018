using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    [SerializeField] private GameObject ball;
    [SerializeField] private GameObject player;

    private GameStates state;
    private Vector3 spawnPoint1, spawnPoint2;
    public static Manager manager;
    public KeyCode[] keyCodes;
    public List<GameObject> PlayerObjects { get; set; }
    public GameObject Ball { get; set; }
    public Ball BallScript { get; set; }
	
	private void Awake () {
		DontDestroyOnLoad(gameObject);

        manager = this;
		PlayerObjects = new List<GameObject>();
		
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
        GameObject player1 = GameObject.Instantiate(player);
        GameObject player2 = GameObject.Instantiate(player);
        PlayerObjects.Add(player1);
        PlayerObjects.Add(player2);

        PlayerObjects[0].GetComponent<PlayerMechanics>().id = 0;
        PlayerObjects[1].GetComponent<PlayerMechanics>().id = 1;
        PlayerObjects[0].GetComponent<PlayerMechanics>().CustomStart();
        PlayerObjects[1].GetComponent<PlayerMechanics>().CustomStart();

        Ball = GameObject.Find("Ball");
        spawnPoint1 = GameObject.Find("Ball Spawn 1").transform.position;
        spawnPoint2 = GameObject.Find("Ball Spawn 2").transform.position;
        var randomPlayer = PlayerObjects[(int)Mathf.Round(Random.Range(0, 1))];
        Ball.transform.position = randomPlayer.transform.position + randomPlayer.transform.forward * 2;
        state = GameStates.Playing;
    }

    private void InitializeGame()
	{
		state = GameStates.Playing;
		SceneManager.LoadScene("MenuScene");
	}

	public GameStates State
	{
		get { return state; }
		set { state = value; }
	}
}
