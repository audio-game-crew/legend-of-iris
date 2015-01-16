﻿using System;
using UnityEngine;

public class ConversationQuest : Quest<ConversationQuest, ConversationQuestDefinition> {

	public ConversationQuest(ConversationQuestDefinition definition) : base(definition) {}

	public Boolean LucyQuiet;

    protected override void _Start() {
		base._Start();
		// Conversation conversation = ConversationManager.create(definition.conversationId);
		// conversation.onConversationEnd += OnConversationEnd;
		// conversation.Play();
		if (LucyQuiet == true) 
        {
			var lucyController = Characters.instance.Lucy.GetComponent<LucyController>();
            lucyController.StopBell();
			//lucyControler.StopAudio ();
        }

		var player = ConversationManager.GetConversationPlayer(definition.conversationId);
        if (player != null)
        {
            SetPlayerMovementLocked(true);
            player.onConversationEnd += OnConversationEnd;
            player.Start();
        } else
        {
            Complete();
        }
	}

    private void SetPlayerMovementLocked(bool locked)
    {
        var player = Characters.instance.Beorn;
        if (player == null)
            return;
        var playerController = player.GetComponent<PlayerController>();
        if (playerController == null)
            return;
        playerController.LockMovement = locked;
    }

	private void OnConversationEnd(ConversationPlayer player) {
        if (player != null)
            player.onConversationEnd -= OnConversationEnd;
        SetPlayerMovementLocked(false);
		Complete();
        if (LucyQuiet)
        {
            var lucyController = Characters.instance.Lucy.GetComponent<LucyController>();
            lucyController.StartBell();
        }
            
	}

}
