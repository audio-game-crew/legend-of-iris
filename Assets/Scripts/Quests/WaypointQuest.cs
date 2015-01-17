using System;
using UnityEngine;

public class WaypointQuest : Quest<WaypointQuest, WaypointQuestDefinition> {

	public int index = 0;

    private float timeoutTimer;
    private float wrongWayTimer;
    private float minTargetDistance;
    private bool playingConversation = false;
    private bool active = false;

    public WaypointQuest(WaypointQuestDefinition definition) : base(definition) { }

    protected override void _Start()
    {
        active = true;
		var lucyController = Characters.instance.Lucy.GetComponent<LucyController>();
		lucyController.StartBell();
		base._Start();
		Next();

        Characters.instance.Lucy.GetComponent<WalkingFollowerScript>().follow = false;
	}

	private void Next() {
		if (index >= definition.waypoints.Count) {
			Complete();
			return;
		}
		Waypoint waypoint = definition.waypoints[index++];
		waypoint.onPlayerEnter += OnPlayerEnter;
        var lucy = Characters.instance.Lucy.GetComponent<LucyController>();
        lucy.GotoLocation(new PositionRotation(waypoint.transform.position, waypoint.transform.rotation));

        // Reset the timers
        timeoutTimer = 0;
        wrongWayTimer = definition.WrongWayTimeout; // Make sure the wrongway conversation always works the first time after a checkpoint
        var player = Characters.instance.Beorn;
        minTargetDistance = (player.transform.position - waypoint.transform.position).magnitude;
	}

	private void OnPlayerEnter(Waypoint waypoint, GameObject player) {
		waypoint.onPlayerEnter -= OnPlayerEnter;
		Next();
	}

    protected override void _Complete()
    {
        Characters.instance.Lucy.GetComponent<WalkingFollowerScript>().follow = true;
		var lucyController = Characters.instance.Lucy.GetComponent<LucyController>();
		lucyController.StopBell();
		active = false;
        base._Complete();
    }

    public override void Update()
    {
        base.Update();
        if (state != State.STARTED)
            return;
        timeoutTimer += Time.deltaTime;
        wrongWayTimer += Time.deltaTime;
        var player = Characters.instance.Beorn;
        var targetDistance = (player.transform.position - definition.waypoints[index-1].transform.position).magnitude;
        //Debug.Log(definition.gameObject.name + ": " + targetDistance + " < " + minTargetDistance + "?");
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
