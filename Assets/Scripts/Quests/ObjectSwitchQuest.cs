using System;
using UnityEngine;

public class ObjectSwitchQuest : Quest<ObjectSwitchQuest, ObjectSwitchQuestDefinition> {

	public ObjectSwitchQuest(ObjectSwitchQuestDefinition definition) : base(definition) {}

    protected override void _Start()
    {
        foreach (var v in definition.turnOff)
        {
            v.SetActive(false);
        }
        foreach (var v in definition.turnOn)
        {
            v.SetActive(true);
        }
        Complete();
	}
}
