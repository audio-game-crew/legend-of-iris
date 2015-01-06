using UnityEngine;
using System.Collections;

public class RotateScript : MonoBehaviour {

    public Vector3 axis = Vector3.forward;
    public float speed = 120f;
    public float period = 8.377581f;
    public float time = 0f;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        time += Time.deltaTime;
        float p = Mathf.PI * period;
        transform.rotation = Quaternion.AngleAxis(speed * (time - (Mathf.Sin(p * time)) / p), axis);
        transform.eulerAngles = transform.eulerAngles.add(0.01f, 0.01f, 0.01f);
	}
}
