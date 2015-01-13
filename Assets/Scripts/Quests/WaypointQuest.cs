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
		base._Start();
		Next();
	}

	private void Next() {
        timeoutTimer = 0;
        wrongWayTimer = 0;
		if (index >= definition.waypoints.Count) {
			Complete();
			return;
		}
		Waypoint waypoint = definition.waypoints[index++];
		waypoint.onPlayerEnter += OnPlayerEnter;
        var lucy = Characters.instance.Lucy.GetComponent<LucyController>();
        lucy.GotoLocation(new PositionRotation(waypoint.transform.position, waypoint.transform.rotation));
        var player = Characters.instance.Beorn;
        minTargetDistance = (player.transform.position - waypoint.transform.position).magnitude;
	}

	private void OnPlayerEnter(Waypoint waypoint, GameObject player) {
		waypoint.onPlayerEnter -= OnPlayerEnter;
		Next();
	}

    protected override void _Complete()
    {
        active = false;
        base._Complete();
    }

    public override void Update()
    {
        base.Update();
        if (!active)
            return;
        timeoutTimer += Time.deltaTime;
        wrongWayTimer += Time.deltaTime;
        var player = Characters.instance.Beorn;
        var targetDistance = (player.transform.position - definition.waypoints[index].transform.position).magnitude;
        if (targetDistance < minTargetDistance)
            targetDistance = minTargetDistance;

        if (definition.TimeoutActive && !playingConversation && timeoutTimer > definition.Timeout)
            PlayConversation(definition.TimeoutConversationID);

        if (definition.WrongWayActive && !playingConversation && wrongWayTimer > definition.WrongWayTimeout && targetDistance - minTargetDistance > definition.WrongWayThreshold)
            PlayConversation(definition.WrongWayConversationID);
    }

    private void PlayConversation(string conversationID)
    {
        if (string.IsNullOrEmpty(conversationID))
            return;
        var player = ConversationManager.GetConversationPlayer(conversationID);
        player.onConversationEnd += s => { timeoutTimer = 0; playingConversation = false; };
        playingConversation = true;
        player.Start();
    }
}
