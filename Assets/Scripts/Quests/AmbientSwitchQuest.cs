using System;
using UnityEngine;

public class AmbientSwitchQuest : Quest<AmbientSwitchQuest, AmbientSwitchQuestDefinition> {

	public AmbientSwitchQuest(AmbientSwitchQuestDefinition definition) : base(definition) {}

    protected override void _Start()
    {
        foreach (var v in definition.turnOff)
        {
            v.ambientEnabled = false;
        }
        foreach (var v in definition.turnOn)
        {
            v.ambientEnabled = true;
        }
        Complete();
	}
}
