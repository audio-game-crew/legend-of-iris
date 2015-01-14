using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class TeleportPlayerQuest : TeleportQuest
{
    public TeleportPlayerQuest(TeleportPlayerQuestDefinition definition)
        : base(definition)
    { }

    protected override void _Start()
    {
        base._Start();
        var walkScript = this.definition.ObjectToTeleport.GetComponent<WalkScript>();
        if (walkScript != null) walkScript.enabled = false;
    }

    protected override void _Complete()
    {
        var walkScript = this.definition.ObjectToTeleport.GetComponent<WalkScript>();
        if (walkScript != null) walkScript.enabled = true;
        base._Complete();
    }
}