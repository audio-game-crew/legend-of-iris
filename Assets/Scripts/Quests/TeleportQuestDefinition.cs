using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class TeleportQuestDefinition : QuestDefinition
{
    public GameObject TargetLocation;
    public GameObject ObjectToTeleport;

    public AnimationCurve TeleportAnimationVerticalDisplacementValue;
    public AnimationCurve TeleportAnimationHorizontalDisplacementEasing;
    public float JumpHeight = 10;
    public float TeleportTime = 2;

    public override Quest Create()
    {
        return new TeleportQuest(this);
    }
}