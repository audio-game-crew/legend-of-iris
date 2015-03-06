using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ConversationQuestDefinition : QuestDefinition {

	public string conversationId;

    public bool LockPlayerMovement = true;
    public bool completeAtMessage = false;
    public int completeAtMessageIndex = 0;

    public List<MessageEvent> messageEvents;

    [Serializable]
    public class MessageEvent
    {
        public int messageID;
        [Tooltip("Triggers on given message end if set to true, otherwise its beginning")]
        public bool onEnd;
        public UnityEvent onTrigger;
    }

	
	override public Quest Create() {
		return new ConversationQuest(this);
	}
	
}
