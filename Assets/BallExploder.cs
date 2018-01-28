using UnityEngine;

public class BallExploder : MonoBehaviour
{
    [SerializeField] private float _explosionStrength;
    [SerializeField] private float _deleteAfterSeconds;
    private ParticleSystem _particleSystem;
    private Transform _ball;

    private void Start()
    {
        _particleSystem = transform.GetChild(0).gameObject.GetComponent<ParticleSystem>();
        _ball = transform.GetChild(1);
        
        _particleSystem.Emit(0);

        for (var i = 0; i < _ball.childCount; i++)
        {
            var child = _ball.GetChild(i);
            child
                .GetComponent<Rigidbody>()
                .AddForce(Random.insideUnitCircle.normalized * _explosionStrength, ForceMode.Impulse);
        }
        
        Invoke("Delete", _deleteAfterSeconds);
    }

    private void Delete() { Destroy(gameObject);  }
}
