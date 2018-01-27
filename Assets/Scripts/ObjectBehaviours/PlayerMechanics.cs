using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The mechanics of the player
/// </summary>
public class PlayerMechanics : MonoBehaviour {

    private float currentCharge;

    private enum PlayerStates {Charging, Standard, FullyCharged};
    private PlayerStates playerState;

    public int id;
    public GameObject player, shootCollider;
    public float radius, position, height;
    public KeyCode leftKey, rightKey, actionKey;
    public float velocity, accelerationRate, dampening;
    public Material[] materialList;
    private Camera camera;
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
            position = 0;
            player.layer = 9;
            camera.cullingMask = 1 << 0 | 1 << 1 | 1 << 2 | 1 << 4 | 1 << 5 | 1 << 8 | 1 << 10;
        }
        
        else
        {
            camera.rect = new Rect(camera.rect.x, 0.0f, camera.rect.width, 0.5f);
            leftKey = Manager.manager.keyCodes[3];
            rightKey = Manager.manager.keyCodes[4];
            actionKey = Manager.manager.keyCodes[5];
            position = 180;
            player.layer = 10;
            camera.cullingMask = 1 << 0 | 1 << 1 | 1 << 2 | 1 << 4 | 1 << 5 | 1 << 8 | 1 << 9;
        }

        player.transform.position = new Vector3(radius, height, 0);
        transform.RotateAround(transform.position, Vector3.up, position);
    }
	
	// Update is called once per frame
	void Update () {
        CheckKeys();
        position += velocity * Time.deltaTime;
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

    void FireShot()
    {
        var shootColliderBehaviour = shootCollider.GetComponent<ShootColliderBehaviour>();

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
        }
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
