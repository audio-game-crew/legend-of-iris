using UnityEngine;
using System.Collections;

public class SpiritController : MonoBehaviour {
	
	private SpiritGeneratorScript generator;
	private float initialSpeed;
	private float speed;
	
	public void Init(SpiritGeneratorScript g, float startingSpeed) {
		Debug.Log ("we call init spirit controller", gameObject);

		this.generator = g;
		this.initialSpeed = startingSpeed;
		this.speed = startingSpeed;
	}
	
	public void Stop() {
		speed = 0;
	}
	
	void FixedUpdate() {
		transform.position += transform.forward*((speed)*Time.deltaTime);
	}
	
	void OnTriggerEnter(Collider other) {

		if (other.tag == "Player" || other.tag == "Spirit") {
			speed = 0;
			generator.SetActive(false);
		}
	}
	
	void OnTriggerExit(Collider other) {

		if (other == generator.spiritLiveArea.collider) {
						// If a spirit isn't in the game anymore we take if off the spirit list
						generator.RemoveSpirit (this.gameObject);
						Destroy (gameObject);


				} else if (other.tag == "Spirit") {
						speed = initialSpeed;
						generator.SetActive (false);
				} else if (other.tag == "Player") {
					// if the player touches a spirit he is pushed down
					var directionToPush = transform.position - other.transform.position;
					Debug.Log ("directionToPush: "+directionToPush);
					other.transform.position += directionToPush*10;
				}
	}
	
	public void changeSpeed(float newSpeed){
		this.speed = newSpeed;
	}
}
