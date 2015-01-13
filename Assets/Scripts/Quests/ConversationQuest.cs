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
        player.onConversationEnd += OnConversationEnd;
        player.Start();
	}

	private void OnConversationEnd(ConversationPlayer player) {
        player.onConversationEnd -= OnConversationEnd;
		Complete();
	}

}
