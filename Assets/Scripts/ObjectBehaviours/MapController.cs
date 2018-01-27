using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour {

    public int maxTilesToRelease = 0;
	public float releaseProbability = 0;
    public int tileDestroyTime = 0;

    [SerializeField]
    private Transform partParent;
    private int tileCount = 0;
    private bool releasing = false;
	
	// Update is called once per frame
	private void Update () 
	{
        if (!releasing)
            return;

        if (Random.Range(0, 100) > 100f - releaseProbability && tileCount < maxTilesToRelease)
        {
            ReleaseTile();
        }
	}

    private IEnumerator DelayedDestroy(Transform t)
    {
        yield return new WaitForSeconds(tileDestroyTime);

        Destroy(t.gameObject);
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
                tileCount++;

                // StartCoroutine(DelayedDestroy(child));

                return;
            }

            index++;
        }
	}

    public bool AutoRelease
    {
        get { return releasing; }
        set
        {
            releasing = value;
        }
    }
}
