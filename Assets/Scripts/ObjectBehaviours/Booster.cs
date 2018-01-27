using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Booster : MonoBehaviour {

	public float boostStrength;

    [SerializeField]
    private MapController mapController;

	private void OnTriggerEnter(Collider col)
	{
		// Add force to ball
		Transform ball = col.transform;

		if (ball.tag != "Ball")
			return;

		Rigidbody rb = ball.GetComponent<Rigidbody> ();

        // Release a random tile
        mapController.ReleaseTile();

		Vector3 direction = rb.velocity.normalized;
		rb.AddForce (direction * boostStrength, ForceMode.Acceleration);
	}
}
