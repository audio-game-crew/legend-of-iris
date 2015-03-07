using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SerialQuest : RecursiveQuest<SerialQuest, SerialQuestDefinition> {

	public SerialQuest(SerialQuestDefinition definition) : base(definition) {}

	override protected void Check() {
		// Check if all quests have been completed
		base.Check();
		// Check if there is quest ready to be started
        var firstNew = children.FirstOrDefault(c => c.state == State.CREATED);
        if (firstNew != null)
            firstNew.Start();
        else
            Complete();
	}

}
