using System.Collections.Generic;
using System;
using UnityEngine;

public class FirstScienceBirdQuestDefinition : QuestDefinition {

	[System.Serializable]
	public class PrefabSpawnSetting {
		public int count = 1;
		public GameObject prefab = null;
	}

	public List<PrefabSpawnSetting> spawnSettings = new List<PrefabSpawnSetting>();
	public float spawnDistance = 10f;
	public float explanationDelay = 10f;
	public float maxExplanationDelay = 30f;

	override public Quest Create() {
		return new FirstScienceBirdQuest(this);
	}
	
}
