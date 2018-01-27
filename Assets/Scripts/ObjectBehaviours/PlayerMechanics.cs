using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The mechanics of the player
/// </summary>
public class PlayerMechanics : MonoBehaviour {

    private const float CHARGE_RATE = 30, MAX_CHARGE = 120, FIRE_RANGE = 5f, MAX_PUSH_FORCE = 1300;
    private float currentCharge;

    private enum PlayerStates {Charging, Standard, FullyCharged};
    private PlayerStates playerState;

    public GameObject player;
    public float radius, position, height;
    public KeyCode leftKey, rightKey, actionKey;
    public float velocity, accelerationRate, dampening;

	// Use this for initialization
	void Start () {
        //player.transform.position = new Vector3(radius * Mathf.Cos(Mathf.Deg2Rad * position), height, radius * Mathf.Sin(Mathf.Deg2Rad * position));
	    
	    var playerObject = new PlayerObject(this.transform.name, gameObject);
        var manager = FindObjectOfType<Manager>();
	    manager.PlayerObjects.Add(playerObject);
    }
	
	// Update is called once per frame
	void Update () {
        CheckKeys();
        position += velocity;
        player.transform.RotateAround(transform.position, Vector3.up, velocity);
        velocity *= dampening;
        if (Mathf.Abs(velocity) < 0.001)
        {
            velocity = 0;
        }

    }


    void CheckKeys()
    {
        if (Input.GetKey(leftKey))
        {
            velocity += accelerationRate;
        }

        if (Input.GetKey(rightKey))
        {
            velocity -= accelerationRate;
        }

        if (Input.GetKey(actionKey))
        {
            ChargeShot();
        }
        else if (Input.GetKeyUp(actionKey) && (playerState == PlayerStates.Charging || playerState == PlayerStates.FullyCharged))
        {
            FireShot();
        }
    }

    void ChargeShot()
    {
        playerState = PlayerStates.Charging;

        if (playerState == PlayerStates.Charging && currentCharge < MAX_CHARGE)
        {
            currentCharge += CHARGE_RATE * Time.deltaTime;
            if (currentCharge >= MAX_CHARGE)
            {
                playerState = PlayerStates.FullyCharged;
            }
        }
    }

    void FireShot()
    {
        float distance = Vector3.Distance(player.transform.position, Manager.manager.Ball.transform.position);

        if (distance <= FIRE_RANGE)
        {
            Vector3 direction = Manager.manager.Ball.transform.position - player.transform.position;
            print(direction);
            direction.Normalize();
            direction.y = -1;
            float magnitude = MAX_PUSH_FORCE * (1f - (0.6f * (distance / FIRE_RANGE)));
            Manager.manager.BallComponent.RigidBody.AddForce(magnitude * direction, ForceMode.Impulse);
            
        }
        playerState = PlayerStates.Standard;
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
