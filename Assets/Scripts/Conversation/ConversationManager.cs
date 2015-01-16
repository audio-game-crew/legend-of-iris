using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[ExecuteInEditMode]
public class ConversationManager : MonoBehaviour {

    public static ConversationManager instance;
    void Awake()
    {
        instance = this;
    }

    [Header("Settings")]
    public bool maxOneConversationActive = true;
    public float minAudioDistance = 10;
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
            foreach (var cp in playingConversations.ToArray())
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
                foreach (ConversationPlayer c in playingConversations.ToArray())
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

    void AddConversation(List<Conversation> newconversations, ConversationImport obj)
    {
        List<ConversationMessage> messages = new List<ConversationMessage>();
        Conversation c = new Conversation();
        c.nameID = obj.nameID;
        c.messageSequence = messages;
        c.hint = "";

        for (int i = 0; i < obj.gameObjects.Count; i++)
        {
            var m = new ConversationMessage();
            m.audioClip = Resources.Load(obj.files[i], typeof(AudioClip)) as AudioClip;
            m.source = GameObject.Find(obj.gameObjects[i]);
            m.sourceName = obj.names[i];
            if (m.source == null)
                Debug.LogWarning("Source \"" + obj.gameObjects[i] + "\" not found");
            if (m.audioClip == null)
                Debug.LogError("Audio Clip \"" + obj.files[i] + "\" not found. With text: \n" + obj.texts[i]);
            m.subtitle = obj.texts[i];
            m.settings.screenAsSource = obj.screen[i];
            m.settings.timeOffset = obj.delays[i];
            m.settings.volume = obj.volumes[i];
            messages.Add(m);
        }

        newconversations.Add(c);
    }

    private const int C_NameID = 0;
    private const int C_GameObject = C_NameID + 1;
    private const int C_Folder = C_GameObject + 1;
    private const int C_Name = C_Folder + 1;
    private const int C_Delay = C_Name + 1;
    private const int C_ScreenAsSource = C_Delay + 1;
    private const int C_Volume = C_ScreenAsSource + 1;
    private const int C_File = C_Volume + 1;
    private const int C_Subtitle = C_File + 1;

    private const int COLUMNS = C_Subtitle + 1;

    private class ConversationImport
    {
        public string nameID = "";
        public List<string> gameObjects = new List<string>();
        public List<string> folders = new List<string>();
        public List<string> names = new List<string>();
        public List<float> delays = new List<float>();
        public List<float> volumes = new List<float>();
        public List<bool> screen = new List<bool>();
        public List<string> texts = new List<string>();
        public List<string> files = new List<string>();

        public ConversationImport(string[] items)
        {
            this.nameID = items[C_NameID];
        }

        public void read(string[] items)
        {
            int i = gameObjects.Count;
            gameObjects.Add(items[C_GameObject]);
            names.Add(items[C_Name]);
            folders.Add(items[C_Folder]);
            delays.Add(float.Parse(items[C_Delay].Replace(",", ".")));
            volumes.Add(float.Parse(items[C_Volume].Replace(",", ".")));
            screen.Add(items[C_ScreenAsSource].Equals("1"));
            texts.Add("");
            
            if (items[C_File].Length > 0) {
                files.Add(items[C_File]);
            } else {
                files.Add(folders[i] + "/" + folders[i] + " - " + nameID + "_" + (i + 1));
            }
        }

        public void addText(string[] items)
        {
            texts[texts.Count - 1] += items[C_Subtitle] + "\n";
        }
    }

	// Use this for initialization
	void FillConversations () {
        List<Conversation> newconversations = new List<Conversation>();
        string t = storyScript.text;
        string[] lines = t.Split('\n');

        bool started = false;
        ConversationImport conversationObject = null;

        bool skippedFirst = false;

        foreach (string line in lines) {
            if (!skippedFirst)
            {
                skippedFirst = true;
                continue;
            }

            string[] items = line.Split(';');
            if (items.Length != COLUMNS) continue;
            if (items[C_NameID].Length > 0)
            {
                if (started)
                {
                    AddConversation(newconversations, conversationObject);
                }
                started = true;
                conversationObject = new ConversationImport(items);
            }
            if (items[1].Length > 0)
            {
                conversationObject.read(items);
            }
            if (items[C_Subtitle].Length > 0)
            {
                conversationObject.addText(items);
            }
        }

        if (started)
        {
            AddConversation(newconversations, conversationObject);
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
