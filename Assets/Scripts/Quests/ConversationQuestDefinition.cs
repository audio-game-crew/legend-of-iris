using System;
using System.Collections.Generic;

public class ConversationQuestDefinition : QuestDefinition {

	public string conversationId;
	
	public Boolean LucyQuiet = false;
    public bool LockPlayerMovement = true;

	
	override public Quest Create() {
		return new ConversationQuest(this);
	}
	
}
