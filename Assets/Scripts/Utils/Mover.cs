using UnityEngine;
using System.Collections;

public class Mover : MonoBehaviour {

	public float speed = 10.0f;
	public Vector3 displacement = new Vector3(1, 1, 1);
	
	private Vector3 startPosition;

	void Awake() {
		startPosition = transform.position;
	}

	// Update is called once per frame
	void Update () {
		transform.position = startPosition + Mathf.Sin(Time.time * speed) * displacement;
	}
}