using System;
using System.Collections.Generic;

public class ConversationQuestDefinition : QuestDefinition {

	public string conversationId;

    public bool LockPlayerMovement = true;

	
	override public Quest Create() {
		return new ConversationQuest(this);
	}
	
}
