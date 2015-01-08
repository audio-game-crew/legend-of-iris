using System;
using UnityEngine;

public class WaypointQuest : Quest<WaypointQuest, WaypointQuestDefinition> {

	public int index = 0;

	public WaypointQuest(WaypointQuestDefinition definition) : base(definition) {}

	override protected void _Start() {
		base._Start();
		Next();
	}

	private void Next() {
		if (index >= definition.waypoints.Count) {
			Complete();
			return;
		}
		Waypoint waypoint = definition.waypoints[index++];
		waypoint.onPlayerEnter += OnPlayerEnter;
	}

	private void OnPlayerEnter(Waypoint waypoint, GameObject player) {
		waypoint.onPlayerEnter -= OnPlayerEnter;
		Next();
	}

}
