using System.Collections.Generic;
using System;
using UnityEngine;

public class FirstScienceBirdQuestDefinition : QuestDefinition {

	public float spawnDistance = 10f;
	public GameObject scienceBirdPrefab = null;
	public int scienceBirdCount = 1;
	public GameObject crapBirdPrefab = null;
	public int crapBirdCount = 3;

	override public Quest Create() {
		return new FirstScienceBirdQuest(this);
	}
	
}
