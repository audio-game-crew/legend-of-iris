using System;
using System.Collections.Generic;
using UnityEngine;

public class ShowUIQuestDefinition : QuestDefinition
{

    public GameObject EndScreen;
    public string ConversationID;

	override public Quest Create() {
        return new ShowUIQuest(this);
	}

}
