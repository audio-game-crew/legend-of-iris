using UnityEngine;

public class Waypoint : MonoBehaviour {

	public delegate void PlayerEvent(Waypoint waypoint, GameObject player);

	static public event PlayerEvent allOnPlayerEnter;
	static public event PlayerEvent allOnPlayerLeave;

	public event PlayerEvent onPlayerEnter;
	public event PlayerEvent onPlayerLeave;

    void OnTriggerEnter(Collider collider) {
        if (collider.gameObject.tag == "Player") {
	        if (allOnPlayerEnter != null) allOnPlayerEnter(this, collider.gameObject);
	        if (onPlayerEnter != null) onPlayerEnter(this, collider.gameObject);
        }
    }

    void OnTriggerExit(Collider collider) {
        if (collider.gameObject.tag == "Player") {
	        if (allOnPlayerLeave != null) allOnPlayerLeave(this, collider.gameObject);
	        if (onPlayerLeave != null) onPlayerLeave(this, collider.gameObject);
        }
    }
}
