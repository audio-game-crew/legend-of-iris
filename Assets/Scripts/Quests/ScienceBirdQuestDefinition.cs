using System.Collections.Generic;
using System;
using UnityEngine;

public class ScienceBirdQuestDefinition : QuestDefinition {

	public GameObject scienceBirdPrefab = null;
	public GameObject crapBirdPrefab = null;
	public float scienceBirdProbability = 0.4f;
	public float mainSpawnDistance = 10f;
	public float crossSpawnDistance = 3f;
	public float spawnInterval = 8f;
	public float explanationDelay = 10f;
	public float maxExplanationDelay = 30f;
	public string conversationId;
	
	override public Quest Create() {
		return new ScienceBirdQuest(this);
	}
	
}
