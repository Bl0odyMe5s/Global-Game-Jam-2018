using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The mechanics of the player
/// </summary>
public class PlayerMechanics : MonoBehaviour {

    public GameObject player;
    public float radius, position, height;
    public KeyCode leftKey, rightKey, actionKey;
    public float velocity, accelerationRate, dampening;

	public float minParticleSpeed;
	public float maxParticleSpeed;

	public ParticleSystem particlesRight, particlesLeft;

	// Use this for initialization
	void Start () {
        player.transform.position = new Vector3(radius * Mathf.Cos(Mathf.Deg2Rad * position), height, radius * Mathf.Sin(Mathf.Deg2Rad * position));
    }
	
	// Update is called once per frame
	void Update () {
        CheckKeys();
        position += velocity;
        transform.rotation = Quaternion.Euler(new Vector3(0, position, 0));
        velocity *= dampening;
        if (Mathf.Abs(velocity) < 0.001)
        {
            velocity = 0;
        }

		// Play particles
		if (velocity > 0.2f) {
			particlesLeft.Stop ();
			particlesRight.Play ();
			particlesRight.startSpeed = Random.Range(minParticleSpeed, Mathf.Pow(velocity, 2f) * maxParticleSpeed);
		} else if (velocity < -0.2f) {
			particlesRight.Stop ();
			particlesLeft.Play ();
			particlesLeft.startSpeed = Random.Range(minParticleSpeed, Mathf.Pow(velocity, 2f) * maxParticleSpeed);
		} else {
			particlesLeft.startSpeed = 0;
			particlesRight.startSpeed = 0;
			particlesLeft.Stop ();
			particlesRight.Stop ();
		}

    }


    void CheckKeys()
    {
		if (Input.GetKey (leftKey)) {
			velocity += accelerationRate;	
		} else if (Input.GetKey (rightKey)) {
			velocity -= accelerationRate;
		}
    }


    public float Radius
    {
        get
        {
            return radius;
        }
        set
        {
            radius = value;
        }
    }

    public float Position
    {
        get
        {
            return position;
        }
        set
        {
            position = value; 
        }
    }
}
