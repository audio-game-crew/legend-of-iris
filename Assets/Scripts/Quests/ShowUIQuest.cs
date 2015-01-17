using System;
using System.Collections.Generic;
using UnityEngine;

public class ShowUIQuest : Quest<ShowUIQuest, ShowUIQuestDefinition> {
    private ConversationPlayer player = null;

    public ShowUIQuest(ShowUIQuestDefinition definition) : base(definition) { }

    protected override void _Start() {
        base._Start();
        definition.EndScreen.SetActive(true);
        if (!string.IsNullOrEmpty(definition.ConversationID))
        {
            player = ConversationManager.GetConversationPlayer(definition.ConversationID);
            player.onConversationEnd += player_onConversationEnd;
            player.Start();
        }
    }

    void player_onConversationEnd(ConversationPlayer player)
    {
        this.Complete();
    }

    protected override void _Complete()
    {
        base._Complete();
        definition.EndScreen.SetActive(false);
        Application.LoadLevel(Application.loadedLevel);
    }

}
