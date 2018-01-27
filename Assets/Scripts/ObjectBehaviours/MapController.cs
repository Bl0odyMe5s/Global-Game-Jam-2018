using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour {

	public int tileCount = 0;
	public float releaseProbability = 0;

    [SerializeField]
    private Transform partParent;
	
	// Update is called once per frame
	private void Update () 
	{
		if(Random.Range(0, 100) > 100f - releaseProbability)
        {
            ReleaseTile();
        }
	}

	public void ReleaseTile()
	{
        int randomIndex = Random.Range(0, partParent.childCount);

        int index = 0;
        foreach(Transform child in partParent)
        {
            if (index == randomIndex)
            {
                // Release tile
                child.parent = null;
                Rigidbody rb = child.gameObject.AddComponent<Rigidbody>();

                return;
            }

            index++;
        }
	}
}
