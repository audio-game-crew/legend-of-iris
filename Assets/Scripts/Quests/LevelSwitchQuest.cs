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
        LevelSwitcher.ActivateLevel(definition.Level);
        base._Start();
    }

    public override void Update()
    {
        Complete();
    }
}
