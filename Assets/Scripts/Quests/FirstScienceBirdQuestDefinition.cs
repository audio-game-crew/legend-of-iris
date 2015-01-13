using System.Collections.Generic;
using System;
using UnityEngine;

public class FirstScienceBirdQuestDefinition : QuestDefinition {

	public float spawnDistance = 10f;
	public GameObject scienceBirdPrefab = null;
	public GameObject crapBirdPrefab = null;

	override public Quest Create() {
		return new FirstScienceBirdQuest(this);
	}
	
}
