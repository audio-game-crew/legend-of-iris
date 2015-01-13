using System;
using UnityEngine;

public class PressAnyKeyQuest : Quest<PressAnyKeyQuest, PressAnyKeyQuestDefinition> {

    private float lastConversationEnd = 0;
    private ConversationPlayer player = null;

    public PressAnyKeyQuest(PressAnyKeyQuestDefinition definition) : base(definition) { }

    protected override void _Start() {
        base._Start();
        StartConversation();
    }

    public override void Update() {
        if (Input.anyKeyDown) {
            Complete();
            return;
        }
        if (Time.time > lastConversationEnd + definition.repeatDelay)
            StartConversation();
    }

    private void StartConversation() {
        if (player != null) return;
        player = ConversationManager.GetConversationPlayer(definition.conversationId);
        player.onConversationEnd += OnConversationEnd;
        player.Start();
    }

    private void OnConversationEnd(ConversationPlayer _player) {
        _player.onConversationEnd -= OnConversationEnd;
        player = null;
        lastConversationEnd = Time.time;
    }

}
