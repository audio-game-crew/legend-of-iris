using System;
using System.Collections.Generic;
using UnityEngine;

public class StepwiseWaypointQuestDefinition : QuestDefinition
{
    public float maxStepDistance = 20f;
	public List<Waypoint> waypoints;
    public GameObject WaypointPrefab;

	override public Quest Create() {
        return new StepwiseWaypointQuest(this);
	}
	
}
