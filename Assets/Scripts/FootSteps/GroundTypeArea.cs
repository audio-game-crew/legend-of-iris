using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider))]
public class GroundTypeArea : MonoBehaviour {

    public Vector3 size;
    public GroundType groundType;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    void OnDrawGizmos()
    {
        Vector3 me = transform.position;
        Vector3 _center = new Vector3(0, (size.y / 2f), 0);
        Vector3 _size = new Vector3(size.x, size.y, size.z);
        Gizmos.color = new Color(1f, 0, 0);
        Gizmos.DrawWireCube(_center + me, _size);
        ((BoxCollider)GetComponent<Collider>()).size = _size;
        ((BoxCollider)GetComponent<Collider>()).center = new Vector3(0, (size.y / 2f), 0);
        ((BoxCollider)GetComponent<Collider>()).isTrigger = true;
    }
}
