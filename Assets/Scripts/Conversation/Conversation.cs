using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using System.Linq;

[System.Serializable]
public class Conversation
{
    [HideInInspector]
    public string name;
    [Tooltip("The ID of the conversation as shown in the script")]
    public string nameID;
    [Tooltip("What happens in the story when this conversation is played?")]
    public string hint;
    public List<ConversationMessage> messageSequence;
}

[System.Serializable]
public class ConversationMessage
{
    [HideInInspector]
    public string name;
    public GameObject source;
    public AudioClip audioClip;
    [Multiline(4)]
    public string subtitle;
    public ConversationMessageSettings settings = new ConversationMessageSettings();
}

[System.Serializable]
public class ConversationMessageSettings
{
    public float timeOffset = 0f;
    [Range(0f, 1f)]
    public float volume = 1f;
    [Range(0.5f, 1.5f)]
    public float pitch = 1f;

    public ConversationMessageSettings()
    {
        timeOffset = 0f;
        volume = 1f;
        pitch = 1f;
    }
}
