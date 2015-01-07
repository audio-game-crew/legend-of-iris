using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[ExecuteInEditMode]
public class ConversationManager : MonoBehaviour {

    public TextAsset storyScript;
    public bool loadStory;
    public List<Conversation> conversations;
    private int messageHintLength = 60;

    void AddConversation(List<Conversation> newconversations, string nameID, List<string> sources, List<string> texts)
    {
        List<Conversation.Message> messages = new List<Conversation.Message>();
        Conversation c = new Conversation();
        c.nameID = nameID;
        c.messageSequence = messages;
        c.hint = "";

        for (int i = 0; i < sources.Count; i++ )
        {
            var m = new Conversation.Message();
            m.audioClip = Resources.Load(c.nameID + "_" + i, typeof(AudioClip)) as AudioClip;
            m.source = GameObject.Find(sources[i]);
            m.subtitle = texts[i];
            messages.Add(m);
        }

        newconversations.Add(c);
    }

	// Use this for initialization
	void FillConversations () {
        List<Conversation> newconversations = new List<Conversation>();
        string t = storyScript.text;
        string[] lines = t.Split('\n');

        bool started = false;
        string nameID = "";
        List<string> sources = null;
        List<string> texts = null;

        foreach (string line in lines) {
            string[] items = line.Split(';');
            if (items.Length != 3) continue;
            if (items[0].Length > 0)
            {
                if (started)
                {
                    AddConversation(newconversations, nameID, sources, texts);
                }
                started = true;
                nameID = items[0];
                sources = new List<string>();
                texts = new List<string>();
            }
            if (items[1].Length > 0)
            {
                sources.Add(items[1]);
                texts.Add("");
            }
            if (items[2].Length > 0)
            {
                texts[texts.Count - 1] += items[2] + "\n";
            }
        }

        if (started)
        {
            AddConversation(newconversations, nameID, sources, texts);
        }

        conversations = newconversations;
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (Application.isEditor)
        {
            if (loadStory)
            {
                FillConversations();
                loadStory = false;
            }

            foreach (Conversation c in conversations)
            {
                bool incomplete = false;
                int index = 0;
                foreach (Conversation.Message m in c.messageSequence)
                {
                    m.name = "";
                    if (m.source == null || m.audioClip == null || m.subtitle.Length == 0)
                    {
                        incomplete = true;
                        m.name = "!Err! ";
                    }

                    string add = m.subtitle.Length > messageHintLength ? "..." : "";
                    string msg = m.subtitle.Substring(0, Mathf.Min(m.subtitle.Length, messageHintLength));
                    m.name += (m.source == null ? "" : m.source.name) + ": " + msg + add;

                    index++;
                }

                c.name = "";
                if (incomplete || c.nameID.Length == 0)
                {
                    c.name = "!Err! ";
                }
                c.name += c.nameID + (c.hint.Length > 0 ? ": " + c.hint : "");
            }
        }
	}
}
