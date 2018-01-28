using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneController : MonoBehaviour {

    public float flySpeed = 0;
    public int flyDuration = 0;

    private bool active = false;

	private void FixedUpdate () {
        if (!active)
            return;

        transform.position += Vector3.up * flySpeed;

        Invoke("StopMoving", flyDuration);
	}

    private void StopMoving()
    {
        active = false;
    }

    public void Fly()
    {
        active = true;
    }
}
