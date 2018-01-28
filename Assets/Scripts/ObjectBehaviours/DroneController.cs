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

        StartCoroutine(StopMoving());
	}

    private IEnumerator StopMoving()
    {
        yield return new WaitForSeconds(flyDuration);

        active = false;
    }

    public void Fly()
    {
        active = true;
    }
}
