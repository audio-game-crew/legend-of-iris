using System;
using System.Collections.Generic;
using UnityEngine;

public class StepwiseWaypointQuestDefinition : QuestDefinition
{
    public float maxStepDistance = 20f;
	public List<Waypoint> waypoints;
    public GameObject WaypointPrefab;

    public string TimeoutConversationID = null;
    public float Timeout = 10;
    public string WrongWayConversationID = null;
    public float WrongWayThreshold = 2;
    public float WrongWayTimeout = 5;

	override public Quest Create() {
        return new StepwiseWaypointQuest(this);
	}

    public bool TimeoutActive
    {
        get { return !string.IsNullOrEmpty(TimeoutConversationID); }
    }

    public bool WrongWayActive
    {
        get { return !string.IsNullOrEmpty(WrongWayConversationID); }
    }
	
}
