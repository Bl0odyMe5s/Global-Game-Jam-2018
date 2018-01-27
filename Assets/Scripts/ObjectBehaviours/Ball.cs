using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

    private Rigidbody rigidBody;

	// Use this for initialization
    void Awake()
    {
        Manager.manager.Ball = this;
        rigidBody = GetComponent<Rigidbody>();
    }


	void Start () {
		Physics.gravity = Vector3.down * 4.0f;
        rigidBody.AddForce(Vector3.down * /*top*/2000, ForceMode.Impulse);

	}
    
	
    public Rigidbody RigidBody
    {
        get { return rigidBody; }
    }

}
