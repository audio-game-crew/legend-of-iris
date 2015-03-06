using System;
using UnityEngine;

public class ConversationQuest : Quest<ConversationQuest, ConversationQuestDefinition> {

	public ConversationQuest(ConversationQuestDefinition definition) : base(definition) {}


    protected override void _Start() {
        base._Start();
		var player = ConversationManager.GetConversationPlayer(definition.conversationId);
        if (player != null)
        {
            SetPlayerMovementLocked(definition.LockPlayerMovement);
            SetPlayerRotationLock(definition.LockPlayerRotation);
            player.onConversationEnd += OnConversationEnd;
            player.onMessageEnd += player_onMessageEnd;
            player.Start();
        } else
        {
            Complete();
        }
	}

    void player_onMessageEnd(ConversationPlayer player, int index)
    {
        if (definition.completeAtMessage)
        {
            if (index == definition.completeAtMessageIndex)
            {
                Complete();
            }
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

    private void SetPlayerRotationLock(bool locked)
    {
        var player = Characters.instance.Beorn;
        if (player == null) return;
        var playerController = player.GetComponent<PlayerController>();
        if (playerController == null) return;
        playerController.LockRotation = locked;
    }

	private void OnConversationEnd(ConversationPlayer player) {
        if (player != null)
            player.onConversationEnd -= OnConversationEnd;
        Debug.Log("Conversation " + definition.name + " ended");
		Complete();
	}

    protected override void _Complete()
    {
        Debug.Log("Conversation quest completed");
        SetPlayerMovementLocked(false);
        SetPlayerRotationLock(false);
        base._Complete();
    }
}
