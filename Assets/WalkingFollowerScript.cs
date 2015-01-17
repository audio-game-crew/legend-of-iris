﻿using UnityEngine;
using System.Collections;

public class WalkingFollowerScript : MonoBehaviour {

    public bool follow;
    public GameObject followee;
    public float minDistance;
    public bool lockY;
    public float convergeSpeed;
    public Vector3 offset;
    private Vector3 toMove;

	// Update is called once per frame
	void FixedUpdate () 
    {
        if (!follow) return;

        toMove = (followee.transform.position + offset) - transform.position;
        if (toMove.magnitude > minDistance)
        {
            toMove -= toMove.normalized * minDistance;
        }
        else
        {
            toMove = Vector3.zero;
        }

        transform.position = transform.position + toMove * Timeg.safeFixedDelta(convergeSpeed);
	}
}