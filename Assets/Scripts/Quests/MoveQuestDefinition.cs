using System;
using System.Collections.Generic;
using UnityEngine;

public class MoveQuestDefinition : QuestDefinition {
    public float MovementThreshold;
    public Direction direction;
    public string FailTimeoutConversation;
    public float FailTimeout = 10;
    public float InitialFailTimeout = 10;
    public string OppositeDirectionConversation;
    public float OppositeDirectionThreshold;
	
	override public Quest Create() {
		return new MoveQuest(this);
	}
}
