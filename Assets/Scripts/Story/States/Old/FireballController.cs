using UnityEngine;
using System.Collections;

public class FireballController : SpellController {
	
	protected override void OnTriggerEnter(Collider other) {
		if (other == Mallum.target.collider) {
			other.GetComponent<PlayerController>().setOnFire(true);
		}
	}
}
