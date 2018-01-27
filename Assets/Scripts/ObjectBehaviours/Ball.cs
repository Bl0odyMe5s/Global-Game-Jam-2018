using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

	public float gravity = 0.4f;
	public float startForce = 2800f;
	public float playerYMargin;

	private float playerY = 0;
    private bool isInitialized = true;

    private Rigidbody rigidBody;

	// Use this for initialization
    void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        Manager.manager.BallScript = GetComponent<Ball>();
    }


	void Start () {
		Physics.gravity = Vector3.down * gravity;

		//Get player Y position from game manager
		Transform player = GameObject.Find ("player").transform;
		playerY = player.position.y;

		GetComponent<Rigidbody> ().AddForce (Vector2.down * startForce, ForceMode.Impulse);
	}
    
	
    public Rigidbody RigidBody
    {
        get { return rigidBody; }
    }

	private void FixedUpdate()
	{
		if (!isInitialized && transform.position.y >= playerY + playerYMargin) {
			gameObject.SetActive (false);
			Debug.Log ("JIJ BENT AF!");
		}
	}

    private void OnCollisionEnter(Collision collision)
    {
        // First contact
        if(isInitialized)
        {
            //isInitialized = false;
        }
    }
}
