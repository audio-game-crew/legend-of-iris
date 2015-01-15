using System;
using System.Collections.Generic;

public class KeyOrderQuestDefinition : QuestDefinition
{
	
	public float repeatDelay = 10f;
	public string conversationId = "T0.1";
    public List<Direction> KeyOrder;

	override public Quest Create() {
        return new KeyOrderQuest(this);
	}

}
