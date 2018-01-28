﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

	public float gravity = 0.4f;
	public float startForce = 3200f;
	public float playerYMargin;

	private float playerY = 0;
    private bool isInitialized = true;

    private Rigidbody rigidBody;
    private bool reachedTop;
    private int shooterType;
    private ScoreBoardManager scoreBoard;

    private GameObject mapObjectRef;
    [SerializeField] private GameObject _explodingBall;
    
    public MeshRenderer Renderer { get; set; }

	// Use this for initialization
    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        Manager.manager.BallScript = GetComponent<Ball>();
        scoreBoard = FindObjectOfType<ScoreBoardManager>();
    }


	private void Start () {
		Physics.gravity = Vector3.down * gravity;

        mapObjectRef = GameObject.Find("map");

        rigidBody.AddForce (Vector3.down * startForce, ForceMode.Impulse);

	    Renderer = GetComponent<MeshRenderer>();
	}

	private void FixedUpdate()
	{
        float maxY = playerY + playerYMargin;

        if (!isInitialized && transform.position.y >= maxY) {
            reachedTop = true;
		}

        // Calculate trajectory between ball and map
        Vector2 trajectory = new Vector2(mapObjectRef.transform.position.x, mapObjectRef.transform.position.z) - new Vector2(transform.position.x, transform.position.z);
        
        // Ball goes out of bounds
        if (trajectory.magnitude > mapObjectRef.transform.lossyScale.x && reachedTop)
        {
            Manager.manager.AddScore(shooterType);
        }
	}

    

    public void Explode()
    {
        Manager.manager.Sounds[1].Play();
        var explodingBall = Instantiate(_explodingBall);
        explodingBall.transform.position = transform.position;

        Renderer.enabled = false;
    }

    private void OnCollisionStay(Collision collision)
    {
        // First contact //is a pretty good movie. 
        if(isInitialized)
        {
            isInitialized = false;
            return;
        }

        if(collision.collider.tag == "Terrain")
        {
            Manager.manager.AddScore(1 - shooterType);
            // Ball is out of bounds by a hole, the shooter loses
            
        }

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
                    scoreBoard.TimerPanel.color = scoreBoard.PlayerColors[0];
                    break;
                case PlayerMechanics.PLAYER_TWO:
                    baseMaterial.color = Color.blue;
                    scoreBoard.TimerPanel.color = scoreBoard.PlayerColors[1];
                    break;
            }
            // Set time not touched time
            Manager.manager.TimeSinceLastTouch = Time.timeSinceLevelLoad;
            scoreBoard.TimerValue = Manager.manager.SecondsUntilRoundStop;

            shooterType = value;
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
}
