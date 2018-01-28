using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Het moest van michael.
/// </summary>
public class TerrainExtender : MonoBehaviour {


    public AudioClip intro;
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && intro != null)
        {
            gameObject.AddComponent<AudioSource>();
            GetComponent<AudioSource>().PlayOneShot(intro);
            intro = null;
        }
    }

}
