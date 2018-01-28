using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Booster : MonoBehaviour {

	public float boostStrength;

    [SerializeField]
    private MapController mapController;

	private void OnTriggerStay(Collider col)
	{
		// Add force to ball
		Transform ball = col.transform;

		if (ball.tag != "Ball")
			return;

		Rigidbody rb = ball.GetComponent<Rigidbody> ();

		Vector3 direction = rb.velocity.normalized;
		rb.AddForce (direction * boostStrength, ForceMode.Acceleration);
	}

    private void OnTriggerEnter(Collider col)
    {
        // Release a random tile
        mapController.ReleaseTile();
        Manager.manager.Sounds[2].Play();
    }
}
