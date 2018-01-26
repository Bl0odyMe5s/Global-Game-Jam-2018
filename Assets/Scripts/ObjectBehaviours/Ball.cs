using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Physics.gravity = Vector3.down * 0.4f;

		GetComponent<Rigidbody> ().AddForce (Vector2.down * 150f, ForceMode.Impulse);
	}
}
