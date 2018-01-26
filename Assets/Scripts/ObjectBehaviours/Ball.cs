using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

	public float gravity = 0.4f;
	public float startForce = 300f;
	public float playerYMargin;

	private float playerY = 0;

	// Use this for initialization
	void Start () {
		Physics.gravity = Vector3.down * gravity;

		//Get player Y position from game manager
		Transform player = GameObject.Find ("player").transform;
		playerY = player.position.y;

		GetComponent<Rigidbody> ().AddForce (Vector2.down * startForce, ForceMode.Impulse);
	}

	private void FixedUpdate()
	{
		if (transform.position.y >= playerY + playerYMargin) {
			gameObject.SetActive (false);
			Debug.Log ("JIJ BENT AF!");
		}
	}
}
