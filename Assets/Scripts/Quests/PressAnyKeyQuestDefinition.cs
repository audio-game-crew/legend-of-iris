using System;
using System.Collections.Generic;

public class PressAnyKeyQuestDefinition : QuestDefinition {
	
	public float repeatDelay = 10f;
	public string conversationId = "T0.1";

	override public Quest Create() {
		return new PressAnyKeyQuest(this);
	}

}
