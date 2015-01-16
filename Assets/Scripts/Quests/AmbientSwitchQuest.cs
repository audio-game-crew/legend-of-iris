using System;
using UnityEngine;

public class AmbientSwitchQuest : Quest<AmbientSwitchQuest, AmbientSwitchQuestDefinition> {

	public AmbientSwitchQuest(AmbientSwitchQuestDefinition definition) : base(definition) {}

    protected override void _Start()
    {
        definition.turnOff.ambientEnabled = false;
        definition.turnOn.ambientEnabled = true;
        Complete();
	}
}
