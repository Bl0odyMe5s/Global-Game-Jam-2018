using UnityEngine;

public class CameraFollower : MonoBehaviour
{
	private GameObject _target;
	
	private void Start()
	{
		_target = transform.parent.gameObject;
		transform.parent = null;
	}

	private void Update()
	{
		transform.position = _target.transform.position;
		transform.LookAt(_target.transform.parent.position);
	}
}
