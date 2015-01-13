using System;
using UnityEngine;

public class ConversationQuest : Quest<ConversationQuest, ConversationQuestDefinition> {

	public ConversationQuest(ConversationQuestDefinition definition) : base(definition) {}

    protected override void _Start() {
		base._Start();
		// Conversation conversation = ConversationManager.create(definition.conversationId);
		// conversation.onConversationEnd += OnConversationEnd;
		// conversation.Play();
		var player = ConversationManager.GetConversationPlayer(definition.conversationId);
        if (player != null)
        {
            player.onConversationEnd += OnConversationEnd;
            player.Start();
        } else
        {
            Complete();
        }
	}

	private void OnConversationEnd(ConversationPlayer player) {
        if (player != null)
            player.onConversationEnd -= OnConversationEnd;
		Complete();
	}

}
