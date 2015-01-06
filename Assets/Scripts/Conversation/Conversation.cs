using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

[System.Serializable]
public class Conversation
{
    [HideInInspector]
    public string name;
    [HideInInspector]
    public bool initialized = false;
    [Tooltip("The ID of the conversation as shown in the script")]
    public string nameID;
    [Tooltip("What happens in the story when this conversation is played?")]
    public string hint;
    public List<Message> messageSequence;

    public void Initialize()
    {
        if (initialized) return;
        initialized = true;
        nameID = "T0.0";
    }

    [System.Serializable]
    public class Message
    {
        [HideInInspector]
        public string name;
        public GameObject source;
        public AudioClip audioClip;
        [Multiline(4)]
        public string subtitle;
        public MessageSettings settings = new MessageSettings();
    }

    [System.Serializable]
    public class MessageSettings
    {
        [HideInInspector]
        public bool initialized = false;
        public float timeOffset = 0f;
        [Range(0f,1f)]
        public float volume = 1f;
        [Range(0f, 1f)]
        public float pitch = 1f;

        public void Initialize()
        {
            if (initialized) return;
            initialized = true;
            timeOffset = 0f;
            volume = 1f;
            pitch = 1f;
        }
    }

    // --------------------------------------------
    
    public delegate void OnMessageStart(int index);
    public delegate void OnMessageEnd(int index);
    private List<AudioPlayer> players;
    private float startTime;
    public OnMessageStart onMessageStartListener;
    public OnMessageEnd onMessageEndListener;
    public int Count
    {
        get { return messageSequence.Count; }
    }

    public void Start()
    {
        startTime = Time.time;
    }

    public void Update(float deltaTime)
    {
        if (PauseManager.paused)
        {
            startTime += deltaTime;
        }
        else
        {

        }
    }

    public void Stop()
    {

    }
}
