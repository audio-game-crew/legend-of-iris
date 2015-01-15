using System.Collections.Generic;
using System;
using UnityEngine;

public class ScienceBirdQuestDefinition : QuestDefinition {

	public GameObject scienceBirdPrefab = null;
	public GameObject crapBirdPrefab = null;
	public float scienceBirdProbability = 0.4f;
	public float birdSpeed = 8f;
	public float mainSpawnDistance = 30f;
	public float crossSpawnDistance = 8f;
	public float despawnDistance = 60f;
	public float spawnInterval = 8f;
	public float explanationDelay = 10f;
	public float maxExplanationDelay = 30f;
	public string explanationConversationId = "T7.2";
	public string successConversationId;
	
	override public Quest Create() {
		return new ScienceBirdQuest(this);
	}
	
}
