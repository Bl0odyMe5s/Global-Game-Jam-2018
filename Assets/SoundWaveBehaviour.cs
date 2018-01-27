using UnityEngine;

public class SoundWaveBehaviour : MonoBehaviour
{
    [SerializeField] private float size;
    [SerializeField] private float growSpeed;

    private void Awake()
    {
        transform.localScale = Vector3.zero;
    }

    private void Update()
    {
        if (transform.localScale.x < size)
        {
            var grow = growSpeed * Time.deltaTime;
            transform.localScale += new Vector3(grow, grow, grow);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
