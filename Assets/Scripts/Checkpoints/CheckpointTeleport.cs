using UnityEngine;
using System.Collections;

public class CheckpointTeleport : MonoBehaviour {
    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            if (CheckpointManager.instance != null)
                CheckpointManager.instance.GotoLastCheckpoint(this.gameObject);
        }
    }
    
}
