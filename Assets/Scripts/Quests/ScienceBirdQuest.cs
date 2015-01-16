using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ScienceBirdQuest : Quest<ScienceBirdQuest, ScienceBirdQuestDefinition> {

	/* This class needs to spawn a science bird somewhere around the player */
	List<GameObject> prefabs = new List<GameObject>();

	float lastExplanationTime;
	float nextExplanationTime;
	ConversationPlayer explanationPlayer = null;

	float nextSpawnTime;

	enum State { COLLECTING, RETURNING, COMPLETING };

	new State state = State.COLLECTING;

	public ScienceBirdQuest(ScienceBirdQuestDefinition definition) : base(definition) {}

    protected override void _Start() {
		// Play explanation
		lastExplanationTime = Time.time;
		nextExplanationTime = lastExplanationTime + definition.explanationDelay;

		nextSpawnTime = Time.time;

		definition.cageWaypoint.onPlayerEnter += OnPlayerEnterCage;

		base._Start();
	}

	protected override void _Complete() {

		foreach (var f in prefabs) {
			if (f == null) continue;
			GameObject.Destroy(f);
		}

		definition.cageWaypoint.onPlayerEnter -= OnPlayerEnterCage;

		base._Complete();
	}

	private float RandomCrossDistance() {
		var sign = Random.value < .5f ? -1 : 1;
		var dist = definition.crossSpawnOffset + Random.Range(-definition.crossSpawnVariation, definition.crossSpawnVariation);
		return sign * dist;
	}

	private GameObject RandomSpawn(GameObject prefab) {
		Vector3 spawnBase = Characters.instance.Beorn.transform.position;
		Vector3 spawnOffset = new Vector3(RandomCrossDistance(), 0, definition.mainSpawnDistance);
		Quaternion spawnRotation = Quaternion.AngleAxis(Random.Range(0, 360), Vector3.up);
		Vector3 spawnLocation = spawnBase - spawnRotation * spawnOffset;
		return UnityEngine.Object.Instantiate(prefab, spawnLocation, spawnRotation) as GameObject;
	}

	private GameObject SpawnRandomBird() {
		int sum = 0;
		foreach (var setting in definition.spawnSettings) sum += setting.weight;
		int index = Random.Range(0, sum);
		foreach (var setting in definition.spawnSettings) {
			if (index >= setting.weight) {
				index -= setting.weight;
				continue;
			}
			var bird = RandomSpawn(setting.prefab);
			prefabs.Add(bird);
			var waypoint = bird.GetComponent<Waypoint>();
			if (setting.prefab.name == "Science Bird")
				waypoint.onPlayerEnter += OnPlayerEnterScienceBird;
			else
				waypoint.onPlayerEnter += OnPlayerEnterCrapBird;
			return bird;
		}
		return null;
	}

	// Science Bird

	private void OnPlayerEnterScienceBird(Waypoint waypoint, GameObject _) {
		if (state != State.COLLECTING) return;
		waypoint.onPlayerEnter -= OnPlayerEnterScienceBird;
		GameObject.Destroy(waypoint.gameObject);
		state = State.COMPLETING;
		var player = ConversationManager.GetConversationPlayer(definition.successConversationId);
		player.onConversationEnd += OnScienceBirdConversationEnd;
		player.Start();
	}

	private void OnScienceBirdConversationEnd(ConversationPlayer player) {
		player.onConversationEnd -= OnScienceBirdConversationEnd;
		Complete();
	}

	// Crap bird

	private void OnPlayerEnterCrapBird(Waypoint waypoint, GameObject _) {
		if (state != State.COLLECTING) return;
		waypoint.onPlayerEnter -= OnPlayerEnterCrapBird;
		GameObject.Destroy(waypoint.gameObject);

		state = State.RETURNING;
        var lucy = Characters.instance.Lucy.GetComponent<LucyController>();
        if (lucy != null)
        	lucy.GotoObject(definition.cageWaypoint.gameObject);

		var player = ConversationManager.GetConversationPlayer("T8.2");
		player.Start();
	}

	private void OnPlayerEnterCage(Waypoint waypoint, GameObject _) {
		if (state != State.RETURNING) return;

		var player = ConversationManager.GetConversationPlayer("T8.3");
		player.onConversationEnd += OnReturnConversationEnd;
		player.Start();
	}

	private void OnReturnConversationEnd(ConversationPlayer player) {
		player.onConversationEnd -= OnReturnConversationEnd;
		state = State.COLLECTING;
	}

	public override void Update() {
		base.Update();

		if (explanationPlayer == null && state == State.COLLECTING) {
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
			SpawnRandomBird();
		}

		// Update bird positions
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

	}

	private void OnExplanationEnd(ConversationPlayer _) {
		explanationPlayer.onConversationEnd -= OnExplanationEnd;
		explanationPlayer = null;
		lastExplanationTime = Time.time;
		nextExplanationTime = lastExplanationTime + definition.explanationDelay;
	}
}