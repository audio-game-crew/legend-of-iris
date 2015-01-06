using UnityEngine;
using System.Collections;

public class CarController : MonoBehaviour {

	private CarGeneratorScript generator;
	private float initialSpeed;
	private float speed;

	public void Init(CarGeneratorScript g, float startingSpeed) {
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
		if (other.tag == "Player" || other.tag == "Car") {
			speed = 0;
			generator.SetActive(false);
		}
	}

	void OnTriggerExit(Collider other) {
		if (other == generator.carLiveArea.collider) {
			// If a car isn't in the game anymore we take if off the car list
			generator.RemoveCar(this.gameObject);
			Destroy (gameObject);

		} else if (other.tag == "Player" || other.tag == "Car") {
			speed = initialSpeed;
			generator.SetActive(true);
		}
	}

	public void changeSpeed(float newSpeed){
		this.speed = newSpeed;
	}
}
