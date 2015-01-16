using System;
using System.Collections.Generic;

public class KeyOrderQuestDefinition : QuestDefinition
{
	
	public float repeatDelay = 10f;
	public string conversationId;
    public List<Direction> KeyOrder = new List<Direction>();
    public bool StartConversationImmediately = false;

	override public Quest Create() {
        return new KeyOrderQuest(this);
	}

}
