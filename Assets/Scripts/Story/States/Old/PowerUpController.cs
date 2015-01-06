using UnityEngine;
using System.Collections;

public class PowerUpController : SpellController {

	protected override void OnTriggerEnter(Collider other) {

		if (other == Mallum.target.collider) {
			Mallum.target.GetComponent<PlayerController>().useGrowl();
			Mallum.GetComponent<MallumController>().getHitByPlayer();
		}
	}
}
