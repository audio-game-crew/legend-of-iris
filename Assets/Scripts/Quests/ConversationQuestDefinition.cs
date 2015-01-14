using System;
using System.Collections.Generic;

public class ConversationQuestDefinition : QuestDefinition {

	public string conversationId;
	
	public Boolean LucyQuiet = false;

	
	override public Quest Create() {
		return new ConversationQuest(this);
	}
	
}
