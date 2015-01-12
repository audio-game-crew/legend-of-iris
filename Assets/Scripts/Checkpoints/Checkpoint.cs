using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour {

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            if (CheckpointManager.instance != null)
                CheckpointManager.instance.SetLastCheckpoint(this);
        }
    }
}
