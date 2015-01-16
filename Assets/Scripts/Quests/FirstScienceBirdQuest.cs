using System;
using System.Collections.Generic;
using UnityEngine;

public class FirstScienceBirdQuest : Quest<FirstScienceBirdQuest, FirstScienceBirdQuestDefinition> {

	public enum State { SEARCHING, TESTING };

	/* This class needs to spawn a science bird somewhere around the player */
	List<GameObject> prefabs = new List<GameObject>();
	float lastExplanationTime;
	float nextExplanationTime;
	ConversationPlayer explanationPlayer = null;

	State state = State.SEARCHING;

	public FirstScienceBirdQuest(FirstScienceBirdQuestDefinition definition) : base(definition) {}

    protected override void _Start() {
		// Play explanation
		lastExplanationTime = Time.time;
		nextExplanationTime = lastExplanationTime + definition.explanationDelay;

		foreach (var setting in definition.spawnSettings) {
			for (int i = 0; i < setting.count; i++) {
				var bird = RandomSpawn(setting.prefab);
				prefabs.Add(bird);
				var waypoint = bird.GetComponent<Waypoint>();
				if (setting.prefab.name == "Science Bird")
					waypoint.onPlayerEnter += OnPlayerEnterScienceBird;
				else
					waypoint.onPlayerEnter += OnPlayerEnterCrapBird;
			}
		}

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
		Vector2 spawnOffset = UnityEngine.Random.insideUnitCircle * definition.spawnDistance;
		Vector3 spawnLocation = spawnBase + new Vector3(spawnOffset.x, 0, spawnOffset.y);
		return UnityEngine.Object.Instantiate(prefab, spawnLocation, Quaternion.identity) as GameObject;
	}

	// Science Bird

	private void OnPlayerEnterScienceBird(Waypoint waypoint, GameObject _) {
		waypoint.onPlayerEnter -= OnPlayerEnterScienceBird;
		GameObject.Destroy(waypoint.gameObject);
		state = State.TESTING;
		var player = ConversationManager.GetConversationPlayer("T7.4");
		player.onConversationEnd += OnScienceBirdConversationEnd;
		player.Start();
	}

	private void OnScienceBirdConversationEnd(ConversationPlayer player) {
		player.onConversationEnd -= OnScienceBirdConversationEnd;
		Complete();
	}

	// Other Bird

	private void OnPlayerEnterCrapBird(Waypoint waypoint, GameObject _) {
		if (state != State.SEARCHING) return;
		waypoint.onPlayerEnter -= OnPlayerEnterCrapBird;
		GameObject.Destroy(waypoint.gameObject);
		var player = ConversationManager.GetConversationPlayer("T7.3");
		player.Start();
	}

	// Trigger explanation

	public override void Update() {
		base.Update();
		// add timer that triggers lucy to explain
		if (explanationPlayer == null && state == State.SEARCHING) {
			if (Input.anyKey) {
				nextExplanationTime = Math.Min(
					Time.time + definition.explanationDelay, 
					lastExplanationTime + definition.maxExplanationDelay
				);
			}

			if (Time.time > nextExplanationTime) {
				explanationPlayer = ConversationManager.GetConversationPlayer("T7.2");
				explanationPlayer.onConversationEnd += OnExplanationEnd;
				explanationPlayer.Start();
			}
		}
	}

	private void OnExplanationEnd(ConversationPlayer _) {
		explanationPlayer.onConversationEnd -= OnExplanationEnd;
		explanationPlayer = null;
		lastExplanationTime = Time.time;
		nextExplanationTime = lastExplanationTime + definition.explanationDelay;
	}
}