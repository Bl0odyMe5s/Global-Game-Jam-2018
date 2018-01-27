using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The mechanics of the player
/// </summary>
public class PlayerMechanics : MonoBehaviour {

    private const float CHARGE_RATE = 30, MAX_CHARGE = 120, FIRE_RANGE = 5f, MAX_PUSH_FORCE = 10;
    private float currentCharge;

    private enum PlayerStates {Charging, Standard, FullyCharged};
    private PlayerStates playerState;

    public int id;
    public GameObject player;
    public float radius, position, height;
    public KeyCode leftKey, rightKey, actionKey;
    public float velocity, accelerationRate, dampening;
    public Material[] materialList;
    private Camera camera;

    private Color color;

    private void Awake()
    {
        var playerObject = new PlayerObject(this.transform.name, gameObject);
        camera = player.transform.GetChild(0).transform.GetChild(0).GetComponent<Camera>();
    }

	public float minParticleSpeed;
	public float maxParticleSpeed;

	public ParticleSystem particlesRight, particlesLeft;

	// Use this for initialization
	public void CustomStart () {
        player.GetComponent<MeshRenderer>().material = materialList[id];
        if (id == 0)
        {
            camera.rect = new Rect(camera.rect.x, 0.5f, camera.rect.width, 0.5f);
            leftKey = Manager.manager.keyCodes[0];
            rightKey = Manager.manager.keyCodes[1];
            actionKey = Manager.manager.keyCodes[2];
            position = 0;
        }

        else
        {
            camera.rect = new Rect(camera.rect.x, 0.0f, camera.rect.width, 0.5f);
            leftKey = Manager.manager.keyCodes[3];
            rightKey = Manager.manager.keyCodes[4];
            actionKey = Manager.manager.keyCodes[5];
            position = 180;
        }

        player.transform.position = new Vector3(radius, height, 0);
        transform.RotateAround(transform.position, Vector3.up, position);
    }
	
	// Update is called once per frame
	void Update () {
        CheckKeys();
        position += velocity;
        transform.RotateAround(transform.position, Vector3.up, velocity);
        velocity *= dampening;
        if (Mathf.Abs(velocity) < 0.001)
        {
            velocity = 0;
        }

		/*
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
		*/

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
            float magnitude = MAX_PUSH_FORCE * (1f - (1 * (distance / FIRE_RANGE)));
            Manager.manager.BallScript.RigidBody.AddForce(magnitude * direction, ForceMode.Impulse);

            // Set color of the ball's base color to the player's color
            Manager.manager.Ball.GetComponent<MeshRenderer>().materials[1].color = color;
        }
        playerState = PlayerStates.Standard;
    }

    public Color Color
    {
        get { return color; }
        set
        {
            color = value;
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
