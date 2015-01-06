using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[Serializable]
public class FollowLucyToWaterState : SimpleFollowLucyState
{
	public override void Start(Story script)
	{
		base.Start(script);
	}
	
	public override void Update(Story script)
	{	
		base.Update(script);
	}
	
	public override void End(Story script)
	{
		script.Grumble.GetComponent<PlayerController> ().setOnFire (false);
	}
	
}