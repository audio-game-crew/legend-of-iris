using UnityEngine;
using System.Collections;

public class EdgeDetector : MonoBehaviour {

	void OnTriggerEnter(Collider col)
    {
        if (EdgeManager.instance == null)
            return;
        EdgeManager.instance.EdgeTriggerEnter(this.collider, col);
    }

    void OnTriggerExit(Collider col)
    {
        if (EdgeManager.instance == null)
            return;
        EdgeManager.instance.EdgeTriggerExit(this.collider, col);
    }
}
