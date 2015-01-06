using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[Serializable]
public class FinalBossState : BaseState
{
	public GameObject Boss;
	public BaseState NextState;
	public BaseState PlayerOnFireState;

	public override void Start(Story script) {
		Boss.GetComponent<MallumController> ().setAwaken (true);
	}

	public override void Update(Story script) {

		if (Boss.GetComponent<MallumController> ().isDead ()) {
			script.LoadState(NextState);
		}

		if (script.Grumble.GetComponent<PlayerController> ().isOnFire ()) {
			script.LoadState(PlayerOnFireState);
		}
	}

	public override void End(Story script) {
		Boss.GetComponent<MallumController> ().setAwaken (false);
	}

}
