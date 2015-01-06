using UnityEngine;
using System.Collections;

public class LucyController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(0.3f,0.6f,1f);
        Gizmos.DrawWireSphere(transform.position, 1f);
    }

    //void OnTriggerEnter (Collider other) {
    //    if (other.gameObject.tag == "Player") {
    //        print ("The player found me!");

    //        while (true) {
    //            Vector3 newPos = new Vector3(Random.Range(-100.0F, -82.0F), 2, Random.Range(-100.0F, -86.0F));
    //            float dist2Player = Vector3.Distance(newPos, other.gameObject.transform.position);
    //            //print (dist2Player);

    //            if (dist2Player > 8) {
    //                transform.position = newPos;
    //                break;
    //            }
    //        }

    //    }
    //}

}
