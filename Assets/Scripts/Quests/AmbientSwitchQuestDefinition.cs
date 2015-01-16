using System;
using System.Collections.Generic;

public class AmbientSwitchQuestDefinition : QuestDefinition {

    public AmbientArea turnOff;
    public AmbientArea turnOn;

	override public Quest Create() {
        return new AmbientSwitchQuest(this);
	}
	
}