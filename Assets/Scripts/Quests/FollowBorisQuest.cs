using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FollowBorisQuest : Quest<FollowBorisQuest, FollowBorisQuestDefinition> {
	
	private LinkedList<Waypoint> steps = new LinkedList<Waypoint>();
	private HashSet<Waypoint> addedWaypoints = new HashSet<Waypoint>();
	Waypoint current;
	
	private float timeoutTimer;
	private float wrongWayTimer;
	private float minTargetDistance;
	private bool playingConversation = false;
	private bool active = false;
	private bool teleporting = false;
	
	public FollowBorisQuest(FollowBorisQuestDefinition definition) : base(definition) { }
	
	protected override void _Start()
	{
		base._Start();
		FillSteps();
		Next();
		var checkpointManager = CheckpointManager.instance;
		if (checkpointManager != null)
		{
			checkpointManager.StartLastCheckpointTeleport += checkpointManager_StartLastCheckpointTeleport;
			checkpointManager.EndLastCheckpointTeleport += checkpointManager_EndLastCheckpointTeleport;
		}
	}
	
	private void FillSteps()
	{
		var lastPosition = Characters.instance.Boris.transform.position;
		var lastRotation = Characters.instance.Boris.transform.rotation;
		foreach (var waypoint in definition.waypoints)
		{
			var newSteps = GetStepsBetween(lastPosition, lastRotation, waypoint);
			foreach (var step in newSteps)
				steps.AddLast(step);
			lastPosition = waypoint.transform.position;
			lastRotation = waypoint.transform.rotation;
		}
		
	}
	
	private IEnumerable<Waypoint> GetStepsBetween(Vector3 lastPosition, Quaternion lastRotation, Waypoint waypoint)
	{
		var steps = new List<Waypoint>();
		
		var targetVec = waypoint.transform.position - lastPosition;
		
		var distance = targetVec.magnitude;
		if (distance > definition.maxStepDistance)
		{
			var stepCount = Mathf.Ceil(distance / definition.maxStepDistance);
			var stepSize = 1 / stepCount;
			for (int i = 1; i < stepCount; i++)
			{
				var percentageDone = stepSize * (float)i;
				var newWaypointGO = (GameObject)GameObject.Instantiate(definition.WaypointPrefab,
				                                                       lastPosition + percentageDone * targetVec,
				                                                       Quaternion.RotateTowards(lastRotation, waypoint.transform.rotation, percentageDone));
				var newWaypoint = newWaypointGO.GetComponent<Waypoint>();
				if (newWaypoint == null)
					Debug.LogError("The waypoint prefab doesn't contain a Waypoint component", newWaypointGO);
				steps.Add(newWaypoint);
				addedWaypoints.Add(newWaypoint);
			}
		}
		steps.Add(waypoint);
		return steps;
	}
	
	private void Next() {
		if (steps.Count == 0) {
			Complete();
			return;
		}
		current = steps.First();
		steps.RemoveFirst();
		current.onPlayerEnter += OnPlayerEnter;
		var Boris = Characters.instance.Boris.GetComponent<BorisController>();
		Boris.GotoLocation(new PositionRotation(current.transform.position, current.transform.rotation));
		
		ResetTimers();
	}
	
	private void ResetTimers()
	{
		// Reset the timers
		timeoutTimer = 0;
		wrongWayTimer = definition.WrongWayTimeout; // Make sure the wrongway conversation always works the first time after a checkpoint
		var player = Characters.instance.Beorn;
		minTargetDistance = (player.transform.position - current.transform.position).magnitude;
	}
	
	private void OnPlayerEnter(Waypoint waypoint, GameObject player) {
		waypoint.onPlayerEnter -= OnPlayerEnter;
		if (addedWaypoints.Contains(waypoint))
		{
			addedWaypoints.Remove(waypoint);
			GameObject.Destroy(waypoint.gameObject);
		}
		Next();
	}
	
	public override void Update()
	{
		base.Update();
		if (state != State.STARTED)
			return;
		timeoutTimer += Time.deltaTime;
		wrongWayTimer += Time.deltaTime;
		var player = Characters.instance.Beorn;
		var targetDistance = (player.transform.position - current.transform.position).magnitude;
		//Debug.Log(definition.gameObject.name + ": " + targetDistance + " < " + minTargetDistance + "?");
		if (targetDistance < minTargetDistance)
			minTargetDistance = targetDistance;
		
		if (timeoutTimer > definition.Timeout)
			PlayTimeoutConversation();
		
		if (definition.WrongWayActive && !playingConversation && wrongWayTimer > definition.WrongWayTimeout && targetDistance - minTargetDistance > definition.WrongWayThreshold)
		{
			PlayWrongWayConversation();
			minTargetDistance = targetDistance; // Reset to make sure it only plays again if the player keeps moving away
		}
	}
	
	private void PlayTimeoutConversation()
	{
		if (playingConversation || teleporting || !definition.TimeoutActive)
			return;
		var player = ConversationManager.GetConversationPlayer(definition.TimeoutConversationID);
		player.onConversationEnd += s => { timeoutTimer = 0; playingConversation = false; };
		playingConversation = true;
		player.Start();
	}
	
	private void PlayWrongWayConversation()
	{
		if (playingConversation || teleporting || !definition.WrongWayActive)
			return;
		var player = ConversationManager.GetConversationPlayer(definition.WrongWayConversationID);
		player.onConversationEnd += s => { wrongWayTimer = 0; playingConversation = false; };
		playingConversation = true;
		player.Start();
	}
	
	
	private void PlayResetConversation()
	{
		if (playingConversation || !definition.ResetActive)
			return;
		var player = ConversationManager.GetConversationPlayer(definition.ResetConversationID);
		player.onConversationEnd += s => { ResetTimers(); playingConversation = false; };
		playingConversation = true;
		player.Start();
	}
	
	private void checkpointManager_StartLastCheckpointTeleport(object sender, GotoCheckpointEventArgs e)
	{
		teleporting = true;
		if (!string.IsNullOrEmpty(e.ConversationID)) // Suppress quest conversation if checkpoint reset already has a conversation
			return;
		PlayResetConversation();
	}
	
	private void checkpointManager_EndLastCheckpointTeleport(object sender, EventArgs e)
	{
		teleporting = false;
		var player = Characters.instance.Beorn;
		if (player == null)
			return;
		var newSteps = GetStepsBetween(player.transform.position, player.transform.rotation, current);
		//Debug.Log("newSteps: " + string.Join(", ", newSteps.Select(s => s.transform.position.ToString()).ToArray()));
		steps.AddFirst(current);
		foreach (var step in newSteps.Reverse())
			steps.AddFirst(step);
		Next();
	}
	
	
	
}
