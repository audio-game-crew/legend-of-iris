using UnityEngine;
using System.Collections;

public class ConversationTrigger : MonoBehaviour {
    public string ConversationID;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == Characters.instance.Beorn)
        {
            CheckpointManager.instance.GotoLastCheckpoint(this, ConversationID);
        }
    }
}
