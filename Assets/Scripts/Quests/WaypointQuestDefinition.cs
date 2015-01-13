using System;
using System.Collections.Generic;

public class WaypointQuestDefinition : QuestDefinition {

	public List<Waypoint> waypoints;
    public string TimeoutConversationID = null;
    public float Timeout = 10;
    public string WrongWayConversationID = null;
    public float WrongWayThreshold = 2;
    public float WrongWayTimeout = 5;
	
	override public Quest Create() {
		return new WaypointQuest(this);
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
