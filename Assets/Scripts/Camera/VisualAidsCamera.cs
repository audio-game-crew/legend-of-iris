using UnityEngine;
using System.Collections;

public class VisualAidsCamera : MonoBehaviour {

    public Transform followeeNormal;
    public Transform followeeOR;

    public Vector3 followeeLocalForward;
    public float rotationConvergeSpeed = 4f;
    public float positionConvergeSpeed = 4f;
    public float scrollSpeed = 4f;
    public float minZoom = 4f;
    public float maxZoom = 10f;
    public float zoom = 0.5f;
    public Vector3 currentForward;

	// Use this for initialization
	void Start () {
        currentForward = followeeNormal.TransformDirection(followeeLocalForward);
	}
	
	// Update is called once per frame
    void FixedUpdate()
    {
        Transform followee = followeeOR.gameObject.activeInHierarchy ? followeeOR : followeeNormal;
        zoom -= scrollSpeed * Input.GetAxis("Mouse ScrollWheel") / 20f;
        zoom = Mathf.Clamp01(zoom);

        float y = maxZoom * zoom + minZoom;
        currentForward += (followee.TransformDirection(followeeLocalForward) - currentForward) * Timeg.safeFixedDelta(rotationConvergeSpeed);
        transform.position += (followee.position.sety(y) - transform.position) * Timeg.safeFixedDelta(positionConvergeSpeed);
        transform.LookAt(followee, currentForward);
        Debug.DrawLine(transform.position, transform.position + currentForward * 5f, Color.red, Time.deltaTime);
	}
}
