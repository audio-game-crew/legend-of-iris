using System;
using System.Collections.Generic;

public class WaypointQuestDefinition : QuestDefinition {

	public List<Waypoint> waypoints;
	
	override public Quest Create() {
		return new WaypointQuest(this);
	}
	
}
