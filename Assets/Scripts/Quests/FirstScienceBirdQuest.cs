using System;
using System.Collections.Generic;
using UnityEngine;

public class FirstScienceBirdQuest : Quest<FirstScienceBirdQuest, FirstScienceBirdQuestDefinition> {

	/* This class needs to spawn a science bird somewhere around the player */
	List<GameObject> prefabs = new List<GameObject>();

	public FirstScienceBirdQuest(FirstScienceBirdQuestDefinition definition) : base(definition) {}

    protected override void _Start() {
		base._Start();

		for (int i = 0; i < definition.scienceBirdCount; i++)
			SpawnScienceBird();

		for (int i = 0; i < definition.crapBirdCount; i++)
			SpawnCrapBird();
	}

	private GameObject RandomSpawn(GameObject prefab) {
		Vector3 spawnBase = Characters.instance.Beorn.transform.position;
		Vector2 spawnOffset = UnityEngine.Random.insideUnitCircle * definition.spawnDistance;
		Vector3 spawnLocation = spawnBase + new Vector3(spawnOffset.x, 0, spawnOffset.y);
		return UnityEngine.Object.Instantiate(prefab, spawnLocation, Quaternion.identity) as GameObject;
	}

	// Science Bird

	private void SpawnScienceBird() {
		var bird = RandomSpawn(definition.scienceBirdPrefab);
		var waypoint = bird.GetComponent<Waypoint>();
		waypoint.onPlayerEnter += OnPlayerEnterScienceBird;
		prefabs.Add(bird);
	}

	private void OnPlayerEnterScienceBird(Waypoint waypoint, GameObject _) {
		waypoint.onPlayerEnter -= OnPlayerEnterScienceBird;
		GameObject.Destroy(waypoint.gameObject);
		var player = ConversationManager.GetConversationPlayer("T7.4");
		player.onConversationEnd += OnScienceBirdConversationEnd;
		player.Start();
	}

	private void OnScienceBirdConversationEnd(ConversationPlayer player) {
		player.onConversationEnd -= OnScienceBirdConversationEnd;
		Complete();
	}

	// Crap bird

	private void SpawnCrapBird() {
		var bird = RandomSpawn(definition.crapBirdPrefab);
		var waypoint = bird.GetComponent<Waypoint>();
		waypoint.onPlayerEnter += OnPlayerEnterCrapBird;
		prefabs.Add(bird);
	}

	private void OnPlayerEnterCrapBird(Waypoint waypoint, GameObject _) {
		waypoint.onPlayerEnter -= OnPlayerEnterCrapBird;
		GameObject.Destroy(waypoint.gameObject);
		var player = ConversationManager.GetConversationPlayer("T7.3");
		player.Start();
	}

	public override void Update() {
		base.Update();

	}
}
