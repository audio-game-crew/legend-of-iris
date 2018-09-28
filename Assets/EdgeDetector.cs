using UnityEngine;
using System.Collections;

public class EdgeDetector : MonoBehaviour {

	void OnTriggerEnter(Collider col)
    {
        if (EdgeManager.instance == null)
            return;
        EdgeManager.instance.EdgeTriggerEnter(this.GetComponent<Collider>(), col);
    }

    void OnTriggerExit(Collider col)
    {
        if (EdgeManager.instance == null)
            return;
        EdgeManager.instance.EdgeTriggerExit(this.GetComponent<Collider>(), col);
    }
}
