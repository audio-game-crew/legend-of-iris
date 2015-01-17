using System;
using UnityEngine;

public class FollowerEnablerQuest : Quest<FollowerEnablerQuest, FollowerEnablerQuestDefinition> {

	public FollowerEnablerQuest(FollowerEnablerQuestDefinition definition) : base(definition) {}

    protected override void _Start()
    {
        foreach (var v in definition.turnOff)
        {
            v.follow = false;
        }
        foreach (var v in definition.turnOn)
        {
            v.followerToEnable.follow = true;
            v.followerToEnable.followee = v.followeeToFollow;
        }
        Complete();
	}
}
