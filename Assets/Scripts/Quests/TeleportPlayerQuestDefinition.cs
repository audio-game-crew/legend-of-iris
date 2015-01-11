using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class TeleportPlayerQuestDefinition : TeleportQuestDefinition
{
    public override Quest Create()
    {
        this.ObjectToTeleport = Characters.instance.Beorn;
        return new TeleportPlayerQuest(this);
    }
}