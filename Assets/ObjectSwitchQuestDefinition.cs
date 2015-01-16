using System.Collections.Generic;
using UnityEngine;

public class ObjectSwitchQuestDefinition : QuestDefinition
{

    public List<GameObject> turnOff;
    public List<GameObject> turnOn;

	override public Quest Create() {
        return new ObjectSwitchQuest(this);
	}
}