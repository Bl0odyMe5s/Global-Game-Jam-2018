using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

	public float gravity = 0.4f;
	public float startForce = 3200f;
	public float playerYMargin;

	private float playerY = 0;
    private bool isInitialized = true;
    private float timeLastTouch;

    private Rigidbody rigidBody;
    private bool reachedTop;
    private int shooterType;

    private GameObject mapObjectRef;

	// Use this for initialization
    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        Manager.manager.BallScript = GetComponent<Ball>();
    }


	private void Start () {
		Physics.gravity = Vector3.down * gravity;

        mapObjectRef = GameObject.Find("map");

        rigidBody.AddForce (Vector3.down * startForce, ForceMode.Impulse);
	}

	private void FixedUpdate()
	{
        float maxY = playerY + playerYMargin;

        if (!isInitialized && transform.position.y >= maxY) {
            reachedTop = true;
		}
	    
	    // When ball doesnt get touched for set amount of seconds the round will end and give a point to the the opposite player
	    //if (timeLastTouch != 0 && Time.timeSinceLevelLoad - timeLastTouch > Manager.manager.SecondsUntilRoundStop) FinishMatch(shooterType == 0 ? 1 : 0);

        // Calculate trajectory between ball and map
        Vector2 trajectory = new Vector2(mapObjectRef.transform.position.x, mapObjectRef.transform.position.z) - new Vector2(transform.position.x, transform.position.z);
        
        // Ball goes out of bounds
        if (trajectory.magnitude > mapObjectRef.transform.lossyScale.x)
        {
            FinishMatch(shooterType);
        }
	}

    private void FinishMatch(int winnerType)
    {
        if (Manager.manager.State == GameStates.Finishing)
            return;

        Manager.manager.PlayerScores[winnerType] += 1;
        Manager.manager.State = GameStates.Finishing;

        GetComponent<Rigidbody>().isKinematic = true;

        //Reset level
        StartCoroutine(DelayedReset());
    }

    private IEnumerator DelayedReset()
    {
        yield return new WaitForSeconds(Manager.manager.ResetDelay);

        Manager.manager.RestartLevel1();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // First contact
        if(isInitialized)
        {
            isInitialized = false;
            return;
        }

        if(collision.collider.tag == "Terrain")
        {
            FinishMatch(shooterType == 0 ? 1 : 0);
        }
    }

    public Rigidbody RigidBody
    {
        get { return rigidBody; }
    }

    public float PlayerY
    {
        set { playerY = value; }
    }

    public int Shooter
    {
        get { return shooterType; }
        set
        {
            Material baseMaterial = Manager.manager.Ball.GetComponent<MeshRenderer>().materials[1];
            switch (value)
            {
                case PlayerMechanics.PLAYER_ONE:
                    baseMaterial.color = Color.red;
                    break;
                case PlayerMechanics.PLAYER_TWO:
                    baseMaterial.color = Color.blue;
                    break;
            }
            
            // Set time not touched time
            timeLastTouch = Time.timeSinceLevelLoad;

            shooterType = value;
        }
    }
}
