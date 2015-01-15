using System;
using System.Collections.Generic;
using UnityEngine;

public class ScienceBirdQuest : Quest<ScienceBirdQuest, ScienceBirdQuestDefinition> {

	/* This class needs to spawn a science bird somewhere around the player */
	List<GameObject> prefabs = new List<GameObject>();

	float lastExplanationTime;
	float nextExplanationTime;
	ConversationPlayer explanationPlayer = null;

	float nextSpawnTime;

	enum State { COLLECTING, RETURNING };

	new State state = State.COLLECTING;

	public ScienceBirdQuest(ScienceBirdQuestDefinition definition) : base(definition) {}

    protected override void _Start() {
		// Play explanation
		lastExplanationTime = Time.time;
		nextExplanationTime = lastExplanationTime + definition.explanationDelay;

		nextSpawnTime = Time.time;

		base._Start();
	}

	protected override void _Complete() {

		foreach (var f in prefabs) {
			if (f == null) continue;
			GameObject.Destroy(f);
		}
		
		base._Complete();
	}

	private GameObject RandomSpawn(GameObject prefab) {
		Vector3 spawnBase = Characters.instance.Beorn.transform.position;
		Vector3 spawnOffset = new Vector3(
			UnityEngine.Random.Range(-definition.crossSpawnDistance, +definition.crossSpawnDistance),
			0,
			definition.mainSpawnDistance
		);
		Quaternion spawnRotation = Quaternion.AngleAxis(UnityEngine.Random.Range(0, 360), Vector3.up);
		Vector3 spawnLocation = spawnBase - spawnRotation * spawnOffset;
		return UnityEngine.Object.Instantiate(prefab, spawnLocation, spawnRotation) as GameObject;
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
		var player = ConversationManager.GetConversationPlayer(definition.successConversationId);
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
		if (state == State.COLLECTING) {
			if (explanationPlayer == null) {
				if (Input.anyKey) {
					nextExplanationTime = Math.Min(
						Time.time + definition.explanationDelay, 
						lastExplanationTime + definition.maxExplanationDelay
					);
				}

				if (Time.time > nextExplanationTime) {
					explanationPlayer = ConversationManager.GetConversationPlayer(definition.explanationConversationId);
					explanationPlayer.onConversationEnd += OnExplanationEnd;
					explanationPlayer.Start();
				}
			}

			if (Time.time > nextSpawnTime) {
				nextSpawnTime = Time.time + definition.spawnInterval;
				if (UnityEngine.Random.Range(0f, 1f) < definition.scienceBirdProbability) {
					SpawnScienceBird();
				} else {
					SpawnCrapBird();
				}
			}

			var remaining = new List<GameObject>();
			foreach (var f in prefabs) {
				if (f == null) continue;
				f.transform.Translate(Vector3.forward * Time.deltaTime * definition.birdSpeed);
				if (Vector3.Distance(Characters.instance.Beorn.transform.position, f.transform.position) < definition.despawnDistance) {
					remaining.Add(f);
				} else {
					GameObject.Destroy(f);
				}
			}
			prefabs = remaining;
		} else if (state == State.RETURNING) {

		}


	}

	private void OnExplanationEnd(ConversationPlayer _) {
		explanationPlayer.onConversationEnd -= OnExplanationEnd;
		explanationPlayer = null;
		lastExplanationTime = Time.time;
		nextExplanationTime = lastExplanationTime + definition.explanationDelay;
	}
}