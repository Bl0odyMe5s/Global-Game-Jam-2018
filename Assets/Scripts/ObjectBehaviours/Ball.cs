using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

	public float gravity = 0.4f;
	public float startForce = 3200f;
	public float playerYMargin;

	private float playerY = 0;
    private bool isInitialized = true;

    private Rigidbody rigidBody;

	// Use this for initialization
    void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        Manager.manager.BallScript = GetComponent<Ball>();
        print(Manager.manager.BallScript);
    }


	void Start () {
		Physics.gravity = Vector3.down * gravity;

		GetComponent<Rigidbody> ().AddForce (Vector3.down * startForce, ForceMode.Acceleration);
	}
    
	
    public Rigidbody RigidBody
    {
        get { return rigidBody; }
    }

    public float PlayerY
    {
        get { return playerY; }
        set { playerY = value; }
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
