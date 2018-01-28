using UnityEngine;

public class CameraFollower : MonoBehaviour
{
	private float _shakeDuration;
	private const float ShakeAmount = 0.05f;
	private const float DecreaseFactor = 1.0f;
	private GameObject _target;

	private void Awake()
	{
		_target = transform.parent.gameObject;
		transform.parent = null;
	}

	private void Update()
	{
		if (Manager.manager.State == GameStates.Initializing) return;

		transform.position = _target.transform.position;
		
		if (_shakeDuration > 0)
		{
			transform.LookAt(Manager.manager.Ball.transform.position + Random.insideUnitSphere * ShakeAmount);
			_shakeDuration -= Time.deltaTime * DecreaseFactor;
		}
		else
		{
			_shakeDuration = 0f;
			transform.LookAt(Manager.manager.Ball.transform.position);
		}
	}
    
	public void Shake(float shakeSeconds)
	{
		_shakeDuration = shakeSeconds;
	}
}
