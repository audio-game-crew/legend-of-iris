using System.Collections.Generic;
using System;
using UnityEngine;

public class ScienceBirdQuestDefinition : QuestDefinition {

	[System.Serializable]
	public class PrefabSpawnSetting {
		public int weight = 1;
		public GameObject prefab = null;
	}

	[Header("Spawn Settings")]
	public List<PrefabSpawnSetting> spawnSettings = new List<PrefabSpawnSetting>();
	public float mainSpawnDistance = 30f;
	public float crossSpawnOffset = 5f;
	public float crossSpawnVariation = 2.5f;
	public float despawnDistance = 60f;
	public float spawnInterval = 8f;

	[Header("Bird")]
	public float birdSpeed = 8f;

	[Header("Returning")]
	public Waypoint cageWaypoint = null;

	[Header("Explanation")]
	public float explanationDelay = 10f;
	public float maxExplanationDelay = 30f;
	public string explanationConversationId = "T7.2";

	[Header("Quest Complete")]
	public string successConversationId;
	
	override public Quest Create() {
		return new ScienceBirdQuest(this);
	}
	
}
