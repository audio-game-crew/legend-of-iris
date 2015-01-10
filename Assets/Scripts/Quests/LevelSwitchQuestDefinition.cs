using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class LevelSwitchQuestDefinition : QuestDefinition
{
    public int Level = 0;

    public override Quest Create()
    {
        return new LevelSwitchQuest(this);
    }
}