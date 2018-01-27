using UnityEngine;

public class ShootColliderBehaviour : MonoBehaviour
{
    public float? Collision { get; set; }
    public bool HasShot { get; set; }

    private void OnTriggerEnter(Collider other) { HandleCollision(other); }
    private void OnTriggerStay(Collider other) { HandleCollision(other); }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("Ball"))
        {
            Collision = null;
            HasShot = false;
        }
    }

    private void HandleCollision(Collider other)
    {
        if (other.transform.CompareTag("Ball")) Collision = Vector3.Distance(other.transform.position, transform.position);
    }
}
