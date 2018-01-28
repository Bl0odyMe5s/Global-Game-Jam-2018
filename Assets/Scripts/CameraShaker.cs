using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    private float _shakeDuration;
	
    private const float ShakeAmount = 0.7f;
    private const float DecreaseFactor = 1.0f;
	
    private Vector3 _originalPosition;

    private void Start()
    {
        _originalPosition = transform.position;
    }

    private void Update()
    {
        if (_shakeDuration > 0)
        {
            transform.localPosition = _originalPosition + Random.insideUnitSphere * ShakeAmount;
            _shakeDuration -= Time.deltaTime * DecreaseFactor;
        }
        else
        {
            _shakeDuration = 0f;
            transform.localPosition = _originalPosition;
        }
    }
    
    public void Shake(float shakeSeconds)
    {
        _shakeDuration = shakeSeconds;
    }
}
