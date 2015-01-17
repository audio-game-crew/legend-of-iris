using System;
using System.Collections.Generic;
using UnityEngine;

public class FollowerEnablerQuestDefinition : QuestDefinition
{
    [System.Serializable]
    public class FolloweePair
    {
        public WalkingFollowerScript followerToEnable;
        public GameObject followeeToFollow;
    }
    public List<WalkingFollowerScript> turnOff;
    public List<FolloweePair> turnOn;

	override public Quest Create() {
        return new FollowerEnablerQuest(this);
	}
	
}