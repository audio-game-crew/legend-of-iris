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
        base._Start();
        LevelSwitcher.ActivateLevel(definition.Level);
    }

    public override void Update()
    {
        Complete();
    }
}
