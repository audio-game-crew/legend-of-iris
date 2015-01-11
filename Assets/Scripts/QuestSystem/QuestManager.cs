using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

public class QuestManager : MonoBehaviour {

	public List<GameObject> startingQuests = new List<GameObject>();
    public List<GameObject> cheatQuests = new List<GameObject>();

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
        // Send the quests update notifications, so they can keep internal timers etc.
        foreach (var quest in quests)
        {
            //Debug.Log("Updating quest", quest.definition.gameObject.name);
            quest.Update();
        }

        var cheatPressed = GetCheatPressed();
        if (cheatPressed.HasValue)
        {
            Debug.Log("Cheat, started quest " + cheatPressed.Value);
            if (cheatQuests.Count > cheatPressed.Value)
                StartQuest(cheatQuests[cheatPressed.Value]);
        }

    }

    public void StartQuest(GameObject questGO)
    {
        foreach(var quest in questGO.GetComponents<QuestDefinition>())
        {
            foreach (QuestDefinition d in quest.GetComponents<QuestDefinition>())
            {
                Quest q = d.Create();
                quests.Add(q);
                q.Start();
            }
        }
    }

    private int? GetCheatPressed()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            if (Input.GetKeyDown(KeyCode.F2))
                return 0;
            if (Input.GetKeyDown(KeyCode.F3))
                return 1;
        }
        return null;
    }

	private static void OnQuestEvent(Quest quest) {
		Debug.Log(quest.state + ": " + quest.definition.gameObject.name);
	}

}
