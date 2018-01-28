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
        if (Manager.manager.State == GameStates.Initializing) return;
		transform.position = _target.transform.position;
		transform.LookAt(Manager.manager.Ball.transform.position);
    }
}
