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

    private GameStates state;
    public List<Vector3> spawnPoints;
    public static Manager manager;
    public KeyCode[] keyCodes;
    public List<GameObject> PlayerObjects { get; set; }
    public GameObject Ball { get; set; }
    public Ball BallScript { get; set; }
	
	private void Awake () {
		DontDestroyOnLoad(gameObject);

        manager = this;
		PlayerObjects = new List<GameObject>();
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
        GameObject player1 = GameObject.Instantiate(player);
        player1.GetComponent<PlayerMechanics>().Color = Color.red;
        player1.GetComponent<PlayerMechanics>().camera.GetComponent<CameraFollower>().enabled = false;
        GameObject.Find("CameraRailPlayer1").GetComponent<CameraRailScript>().cam = player1.GetComponent<PlayerMechanics>().camera.GetComponent<Camera>();
        GameObject player2 = GameObject.Instantiate(player);
        player2.GetComponent<PlayerMechanics>().Color = Color.blue;
        player2.GetComponent<PlayerMechanics>().camera.GetComponent<CameraFollower>().enabled = false;
        GameObject.Find("CameraRailPlayer2").GetComponent<CameraRailScript>().cam = player2.GetComponent<PlayerMechanics>().camera.GetComponent<Camera>();

        PlayerObjects.Add(player1);
        PlayerObjects.Add(player2);

        PlayerObjects[0].GetComponent<PlayerMechanics>().id = 0;
        PlayerObjects[1].GetComponent<PlayerMechanics>().id = 1;
        PlayerObjects[0].GetComponent<PlayerMechanics>().CustomStart();
        PlayerObjects[1].GetComponent<PlayerMechanics>().CustomStart();

        Ball = GameObject.Find("Ball");
        Ball.GetComponent<Rigidbody>().isKinematic = true;
        int random = (int)Mathf.Round(Random.Range(0, 2));
        spawnPoints.Add(GameObject.Find("Ball Spawn 1").transform.position);
        spawnPoints.Add(GameObject.Find("Ball Spawn 2").transform.position);
        Ball.transform.position = spawnPoints[random];
        BallScript.PlayerY = player.transform.position.y;
        state = GameStates.Introduction;
    }

    public IEnumerator StartLevel1()
    {
        GameObject.Find("CameraRailPlayer2").GetComponent<CameraRailScript>().enabled = false;
        GameObject.Find("CameraRailPlayer2").GetComponent<CameraRailScript>().enabled = false;

        yield return new WaitForSeconds(1);
        PlayerObjects[0].GetComponent<PlayerMechanics>().camera.GetComponent<CameraFollower>().enabled = true;
        PlayerObjects[1].GetComponent<PlayerMechanics>().camera.GetComponent<CameraFollower>().enabled = true;

        yield return new WaitForSeconds(1);
        state = GameStates.Playing;
        Ball.GetComponent<Rigidbody>().isKinematic = false;
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

    public GameObject SoundWave
    {
        get { return soundWave; }
    }
}
