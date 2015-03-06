using System;
using System.Collections.Generic;

public class KeyOrderQuestDefinition : QuestDefinition
{
	
	public float repeatDelay = 10f;
	public string conversationId;
    public List<Direction> KeyOrder = new List<Direction>();
    public float StartConversationFirstDelay = 0;

	override public Quest Create() {
        return new KeyOrderQuest(this);
	}

}
