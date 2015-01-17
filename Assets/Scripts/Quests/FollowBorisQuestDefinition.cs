using System;
using System.Collections.Generic;
using UnityEngine;

public class FollowBorisQuestDefinition : QuestDefinition
{
	public float maxStepDistance = 20f;
	public List<Waypoint> waypoints;
	public GameObject WaypointPrefab;

    [Header("Conversation when boris arrives at location.")]
    public string arrivedAtLocationConversationID = null;
	
	[Header("Conversation when the player takes too long.")]
    public string TimeoutConversationID = null;
    public float Timeout = 10;
    public float InitialTimeout = 10;
	[Header("Conversation when the player moves the wrong way.")]
	public string WrongWayConversationID = null;
	public float WrongWayThreshold = 2;
	public float WrongWayTimeout = 5;
	[Header("Conversation when the player hits the water and is reset")]
	public string ResetConversationID = null;
	
	
	override public Quest Create() {
		return new FollowBorisQuest(this);
	}
	
	public bool TimeoutActive
	{
		get { return !string.IsNullOrEmpty(TimeoutConversationID); }
	}
	
	public bool WrongWayActive
	{
		get { return !string.IsNullOrEmpty(WrongWayConversationID); }
	}
	
	public bool ResetActive
	{
		get { return !string.IsNullOrEmpty(ResetConversationID); }
	}
	
}
