using System.Collections.Generic;
using UnityEngine;

public class ButtonBehaviour : MonoBehaviour {
    public void ResetButton()
    {
        Manager.manager.PlayerObjects = new List<GameObject>();
        Manager.manager.spawnPoints = new List<Vector3>();
        Manager.manager.SpawnLevel1();
    }
}
