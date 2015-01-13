using System;
using System.Collections.Generic;
using UnityEngine;

public class StepwiseWaypointQuest : Quest<StepwiseWaypointQuest, StepwiseWaypointQuestDefinition> {

    private Queue<Waypoint> steps = new Queue<Waypoint>();
    private HashSet<Waypoint> addedWaypoints = new HashSet<Waypoint>();
    Waypoint current;

    private float timeoutTimer;
    private float wrongWayTimer;
    private float minTargetDistance;
    private bool playingConversation = false;
    private bool active = false;

    public StepwiseWaypointQuest(StepwiseWaypointQuestDefinition definition) : base(definition) { }

    protected override void _Start()
    {
		base._Start();
        FillSteps();
		Next();
	}

    private void FillSteps()
    {
        var lastPosition = Characters.instance.Lucy.transform.position;
        var lastRotation = Characters.instance.Lucy.transform.rotation;
        foreach (var waypoint in definition.waypoints)
        {
            var targetVec = waypoint.transform.position - lastPosition;

            var distance = targetVec.magnitude;
            if (distance < definition.maxStepDistance) // We can just put the final position as the step
                steps.Enqueue(waypoint);
            else
            {
                var stepCount = Mathf.Ceil(distance / definition.maxStepDistance);
                var stepSize = 1 / stepCount;
                for (int i = 1; i <= stepCount; i++)
                {
                    var percentageDone = stepSize * (float)i;
                    var newWaypointGO = (GameObject)GameObject.Instantiate(definition.WaypointPrefab, 
                        lastPosition + percentageDone * targetVec,
                        Quaternion.RotateTowards(lastRotation, waypoint.transform.rotation, percentageDone));
                    var newWaypoint = newWaypointGO.GetComponent<Waypoint>();
                    if (newWaypoint == null)
                        Debug.LogError("The waypoint prefab doesn't contain a Waypoint component", newWaypointGO);
                    steps.Enqueue(newWaypoint);
                    addedWaypoints.Add(newWaypoint);
                }
            }
            lastPosition = waypoint.transform.position;
            lastRotation = waypoint.transform.rotation;
        }

    }

	private void Next() {
		if (steps.Count == 0) {
			Complete();
			return;
		}
        current = steps.Dequeue();
        current.onPlayerEnter += OnPlayerEnter;
        var lucy = Characters.instance.Lucy.GetComponent<LucyController>();
        lucy.GotoLocation(new PositionRotation(current.transform.position, current.transform.rotation));

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
        Debug.Log(definition.gameObject.name + ": " + targetDistance + " < " + minTargetDistance + "?");
        if (targetDistance < minTargetDistance)
            minTargetDistance = targetDistance;

        if (definition.TimeoutActive && !playingConversation && timeoutTimer > definition.Timeout)
            PlayTimeoutConversation();

        if (definition.WrongWayActive && !playingConversation && wrongWayTimer > definition.WrongWayTimeout && targetDistance - minTargetDistance > definition.WrongWayThreshold)
        {
            PlayWrongWayConversation();
            minTargetDistance = targetDistance; // Reset to make sure it only plays again if the player keeps moving away
        }
    }

    private void PlayTimeoutConversation()
    {
        if (string.IsNullOrEmpty(definition.TimeoutConversationID))
            return;
        var player = ConversationManager.GetConversationPlayer(definition.TimeoutConversationID);
        player.onConversationEnd += s => { timeoutTimer = 0; playingConversation = false; };
        playingConversation = true;
        player.Start();
    }

    private void PlayWrongWayConversation()
    {
        if (string.IsNullOrEmpty(definition.WrongWayConversationID))
            return;
        var player = ConversationManager.GetConversationPlayer(definition.WrongWayConversationID);
        player.onConversationEnd += s => { wrongWayTimer = 0; playingConversation = false; };
        playingConversation = true;
        player.Start();
    }
}
