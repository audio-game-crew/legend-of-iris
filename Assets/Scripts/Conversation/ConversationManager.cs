using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[ExecuteInEditMode]
public class ConversationManager : MonoBehaviour {

    private static ConversationManager instance;
    void Awake()
    {
        instance = this;
    }

    [Header("Settings")]
    public bool maxOneConversationActive = true;
    public List<Conversation> conversations;

    [Header("Story script importer")]
    public TextAsset storyScript;
    public bool importConversationsFromStory;
    private int messageHintLength = 60;

    [Header("Debug")]
    public int playingConversationsCount = 0;

    private List<ConversationPlayer> playingConversations = new List<ConversationPlayer>();

    // Update is called once per frame
    void Update()
    {
        if (Application.isEditor)
        {
            CheckConversations();
        }

        // check if player wants to skip
        if (Input.GetKeyUp(KeyCode.Space))
        {
            foreach (var cp in playingConversations)
            {
                cp.Skip();
            }
        }

        // update the conversations
        // remove if finished
        for (int i = 0; i < playingConversations.Count; i++)
        {
            ConversationPlayer cp = playingConversations[i];
            cp.Update();

            if (cp.IsFinished())
            {
                playingConversations.Remove(cp);
                i--;
            }
        }

        playingConversationsCount = playingConversations.Count;
    }

    public static ConversationPlayer GetConversationPlayer(string conversationID)
    {
        return instance.getConversationPlayer(conversationID);
    }

    private ConversationPlayer getConversationPlayer(string conversationID)
    {
        Conversation toPlay = null;
        toPlay = conversations.FirstOrDefault(c => c.nameID.Equals(conversationID));
        if (toPlay == null && !string.IsNullOrEmpty(conversationID))
        { // If no conversation is found with that ID, select a random conversation that starts with it.
           var options = conversations.Where(c => c.nameID.StartsWith(conversationID));
           if (options.Any())
               toPlay = options.ToArray()[Random.Range(0, options.Count())];
        }

        if (toPlay != null)
        {
            if (maxOneConversationActive)
            {
                foreach (ConversationPlayer c in playingConversations)
                {
                    c.Skip();
                }
            }
            ConversationPlayer cp = new ConversationPlayer(toPlay);
            playingConversations.Add(cp);
            return cp;
        }
        else
        {
            Debug.LogError("Conversation with ID '" + conversationID + "' not found");
        }
        return null;
    }

    void AddConversation(List<Conversation> newconversations, string nameID, List<string> sources, List<string> names, List<string> texts)
    {
        List<ConversationMessage> messages = new List<ConversationMessage>();
        Conversation c = new Conversation();
        c.nameID = nameID;
        c.messageSequence = messages;
        c.hint = "";

        for (int i = 0; i < sources.Count; i++ )
        {
            var m = new ConversationMessage();
            m.audioClip = Resources.Load(sources[i] + "/" + sources[i] + " - " + c.nameID + "_" + (i + 1), typeof(AudioClip)) as AudioClip;
            m.source = GameObject.Find(sources[i]);
            m.sourceName = names[i];
            if (m.source == null)
                Debug.LogWarning("Source \"" + sources[i] + "\" not found");
            if (m.audioClip == null)
                Debug.LogWarning("Audio Clip \"" + sources[i] + "/" + sources[i] + " - " + c.nameID + "_" + (i + 1) + "\" not found. With text: \n" + texts[i]);
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
        List<string> names = null;
        List<string> texts = null;

        foreach (string line in lines) {
            string[] items = line.Split(';');
            if (items.Length != 4) continue;
            if (items[0].Length > 0)
            {
                if (started)
                {
                    AddConversation(newconversations, nameID, sources, names, texts);
                }
                started = true;
                nameID = items[0];
                sources = new List<string>();
                names = new List<string>();
                texts = new List<string>();
            }
            if (items[1].Length > 0)
            {
                sources.Add(items[1]);
                names.Add(items[2]);
                texts.Add("");
            }
            if (items[3].Length > 0)
            {
                texts[texts.Count - 1] += items[3] + "\n";
            }
        }

        if (started)
        {
            AddConversation(newconversations, nameID, sources, names, texts);
        }

        conversations = newconversations;
	}

    void CheckConversations()
    {
        if (importConversationsFromStory)
        {
            FillConversations();
            importConversationsFromStory = false;
        }

        foreach (Conversation c in conversations)
        {
            bool incomplete = false;
            int index = 0;
            foreach (ConversationMessage m in c.messageSequence)
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
