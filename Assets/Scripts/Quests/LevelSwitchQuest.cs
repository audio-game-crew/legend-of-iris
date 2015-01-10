using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class LevelSwitchQuest : Quest<LevelSwitchQuest, LevelSwitchQuestDefinition>
{

    public LevelSwitchQuest(LevelSwitchQuestDefinition definition) : base(definition) { }

    protected override void _Start()
    {
        Complete();
        base._Start();
    }

    protected override void _Complete()
    {
        LevelSwitcher.ActivateLevel(definition.Level);
        base._Complete();
    }
}
