using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class TeleportQuest : Quest<TeleportQuest, TeleportQuestDefinition>
{
    private PositionRotation start;
    private PositionRotation target;
    private float time;

    public TeleportQuest(TeleportQuestDefinition definition) : base(definition) { }

    protected override void _Start()
    {
        start = new PositionRotation(definition.ObjectToTeleport);
        target = new PositionRotation(definition.TargetLocation);
        time = 0;
    }

    public override void Update()
    {
        Debug.Log("Updating");
        time += Time.deltaTime;
        var progress = time / definition.TeleportTime;
        var bounce = Ease.loopBounce(progress / 2f);
        if (progress > 1)
        {
            definition.ObjectToTeleport.transform.position = definition.TargetLocation.transform.position;
            definition.ObjectToTeleport.transform.rotation = definition.TargetLocation.transform.rotation;
            Complete();
        } else
        {
            var newPosRot = PositionRotation.Interpolate(start, target, progress, definition.TeleportAnimationHorizontalDisplacementEasing);
            definition.ObjectToTeleport.transform.position = newPosRot.Position.addy(definition.TeleportAnimationVerticalDisplacementValue.Evaluate(bounce) * definition.JumpHeight);
            definition.ObjectToTeleport.transform.rotation = newPosRot.Rotation;
        }
        base.Update();
    }
}
