using UnityEngine;
using System.Collections;

public class Rotator : MonoBehaviour {

	public float speed = 10.0f;
	public Vector3 rotation = new Vector3(1, 1, 1);

	// Update is called once per frame
	void Update () {
		transform.rotation = transform.rotation * Quaternion.Euler(rotation * speed * Time.deltaTime);
	}
}