using System;
using System.Collections.Generic;

public class AmbientSwitchQuestDefinition : QuestDefinition {

    public List<AmbientArea> turnOff;
    public List<AmbientArea> turnOn;

	override public Quest Create() {
        return new AmbientSwitchQuest(this);
	}
	
}