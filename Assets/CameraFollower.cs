using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    private GameObject _target;
	
	private void Start()
	{
        _target = transform.parent.gameObject;
	}

	private void Update()
	{
        if (Manager.manager.State == GameStates.Initializing)
        {
            return;
        }
		transform.position = _target.transform.position;
		transform.LookAt(_target.transform.parent.position);
        var lookPos = Manager.manager.Ball.transform.position - transform.position;
        lookPos.z = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, 0.5f * Time.deltaTime);

    }
}
