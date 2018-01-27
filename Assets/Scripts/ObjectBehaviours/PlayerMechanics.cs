using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The mechanics of the player
/// </summary>
public class PlayerMechanics : MonoBehaviour {

    public const int PLAYER_ONE = 0;
    public const int PLAYER_TWO = 1;

    private int playerType;

    private const float CHARGE_RATE = 30, MAX_CHARGE = 120, FIRE_RANGE = 5f, MAX_PUSH_FORCE = 10;
    private float currentCharge;

    private enum PlayerStates {Charging, Standard, FullyCharged};
    private PlayerStates playerState;

    public int id;
    public GameObject player, shootCollider;
    public float radius, position, height;
    public KeyCode leftKey, rightKey, actionKey;
    public float velocity, accelerationRate, dampening;
    public Material[] materialList;
    public Camera camera;
    [SerializeField] private float chargeMovementspeed;
    [SerializeField] private float minCharge, maxCharge, chargeSpeed;

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
            player.layer = 9;
            camera.cullingMask = 1 << 0 | 1 << 1 | 1 << 2 | 1 << 4 | 1 << 5 | 1 << 8 | 1 << 10;
            position = 90;
        }
        
        else
        {
            camera.rect = new Rect(camera.rect.x, 0.0f, camera.rect.width, 0.5f);
            leftKey = Manager.manager.keyCodes[3];
            rightKey = Manager.manager.keyCodes[4];
            actionKey = Manager.manager.keyCodes[5];
            player.layer = 10;
            camera.cullingMask = 1 << 0 | 1 << 1 | 1 << 2 | 1 << 4 | 1 << 5 | 1 << 8 | 1 << 9;
            position = -90;
        }

        player.transform.position = new Vector3(radius, height, 0);
        transform.RotateAround(transform.position, Vector3.up, position);
    }
    // Update is called once per frame
    void Update() {
        if (Manager.manager.State == GameStates.Playing)
        {
            CheckKeys();
        }
        position += velocity * Time.deltaTime;
        transform.RotateAround(transform.position, Vector3.up, velocity);
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

    private void CheckKeys()
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

    private void ChargeShot()
    {
        playerState = PlayerStates.Charging;
        velocity = Mathf.Clamp(velocity, -chargeMovementspeed, chargeMovementspeed);

        if (playerState == PlayerStates.Charging && currentCharge < maxCharge)
        {
            currentCharge += chargeSpeed * Time.deltaTime;
            if (currentCharge >= maxCharge)
            {
                playerState = PlayerStates.FullyCharged;
            }
        }
    }

    private void FireShot()
    {
        var shootColliderBehaviour = shootCollider.GetComponent<ShootColliderBehaviour>();

        // Return if the ball is above the player
        if (Manager.manager.Ball.transform.position.y > transform.position.y)
            return;

        if (shootColliderBehaviour.Collision != null && !shootColliderBehaviour.HasShot)
        {
            shootColliderBehaviour.HasShot = true;

            var ball = Manager.manager.Ball;
            var ballScript = Manager.manager.BallScript;
            var offsetToBall = ball.transform.position - player.transform.position;
            var pushForce = Mathf.Clamp(currentCharge, minCharge, maxCharge);

            ballScript.RigidBody.AddForce(offsetToBall.normalized * pushForce, ForceMode.Impulse);

            var soundWave = Instantiate(Manager.manager.SoundWave);
            soundWave.transform.position = player.transform.position;
            currentCharge = 0;

            // Set color of the ball's base color to the player's color
            Manager.manager.Ball.GetComponent<Ball>().Shooter = playerType;
        }
    }

    public int PlayerType
    {
        get { return playerType; }
        set
        {
            switch(value)
            {
                case PLAYER_ONE:
                    color = Color.red;
                    break;
                case PLAYER_TWO:
                    color = Color.blue;
                    break;
            }

            playerType = value;
        }
    }

    public Color Color
    {
        get { return color; }
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
