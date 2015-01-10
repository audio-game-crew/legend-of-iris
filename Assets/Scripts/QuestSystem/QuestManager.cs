using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

public class QuestManager : MonoBehaviour {

	public List<GameObject> startingQuests = new List<GameObject>();

	public List<Quest> quests = new List<Quest>();

	void Start () {
		// Register handlers on all quests for logging purposes
		Quest.onAnyQuestStart += OnQuestEvent;
		Quest.onAnyQuestComplete += OnQuestEvent;

		// Start all starting quests
		foreach (GameObject o in startingQuests) {
			foreach (QuestDefinition d in o.GetComponents<QuestDefinition>()) {
				Quest q = d.Create();
				quests.Add(q);
				q.Start();
			}
		}
	}

    void Update()
    {
        Debug.Log("Updating quests" + String.Join(",", quests.Select(q => q.ToString()).ToArray()));
        // Send the quests update notifications, so they can keep internal timers etc.
        foreach (var quest in quests)
        {
            Debug.Log("Updating quest", quest.definition);
            quest.Update();
        }
    }

	private static void OnQuestEvent(Quest quest) {
		Debug.Log(quest.state + ": " + quest.definition.gameObject.name);
	}

}
