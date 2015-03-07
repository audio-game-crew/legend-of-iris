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
            player.ConversationEnd += OnConversationEnd;
            player.MessageEnd += player_onMessageEnd;
            player.MessageStart += player_onMessageStart;
            player.Start();
        } else
        {
            Complete();
        }
	}

    void player_onMessageStart(ConversationPlayer player, int index)
    {
        foreach (var e in definition.messageEvents)
        {
            if (e.messageID == index && !e.onEnd)
            {
                e.onTrigger.Invoke();
            }
        }
    }

    void player_onMessageEnd(ConversationPlayer player, int index)
    {
        foreach (var e in definition.messageEvents)
        {
            if (e.messageID == index && e.onEnd)
            {
                e.onTrigger.Invoke();
            }
        }
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
        if (state != State.COMPLETED)
            Complete();
	}

    protected override void _Complete()
    {
        //Debug.Log("Conversation quest completed");
        SetPlayerMovementLocked(false);
        SetPlayerRotationLock(false);
        base._Complete();
    }
}
