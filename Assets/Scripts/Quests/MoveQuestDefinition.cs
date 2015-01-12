using System;
using System.Collections.Generic;
using UnityEngine;

public class MoveQuestDefinition : QuestDefinition {
    public float MovementThreshold;
    public Direction direction;
	
	override public Quest Create() {
		return new MoveQuest(this);
	}
	
    public enum Direction
    {
        Forward, Backward, Left, Right
    }
}
