using System;
using System.Collections.Generic;

public class ConversationQuestDefinition : QuestDefinition {

	public string conversationId;

    public bool LockPlayerMovement = true;
    public bool completeAtMessage = false;
    public int completeAtMessageIndex = 0;

	
	override public Quest Create() {
		return new ConversationQuest(this);
	}
	
}
