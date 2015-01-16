using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class FollowScript : MonoBehaviour {

    public GameObject followee;
    public Vector3 offset;
	
	// Update is called once per frame
	void Update () {
        if (followee != null)
        transform.position = followee.transform.position + offset;
	}
}
