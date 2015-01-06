using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class ConversationManager : MonoBehaviour {

    public List<Conversation> conversations;
    private int messageHintLength = 60;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (Application.isEditor)
        {
            foreach (Conversation c in conversations)
            {
                c.Initialize();
                bool incomplete = false;
                int index = 0;
                foreach (Conversation.Message m in c.messageSequence)
                {
                    m.settings.Initialize();

                    if (m.audioClip == null)
                    {
                        m.audioClip = Resources.Load(c.nameID + "_" + index, typeof(AudioClip)) as AudioClip;
                    }

                    if (m.source != null && m.audioClip != null && m.subtitle.Length > 0)
                    {
                        string add = m.subtitle.Length > messageHintLength ? "..." : "";
                        string msg = m.subtitle.Substring(0, Mathf.Min(m.subtitle.Length, messageHintLength));
                        m.name = m.source.name + ": " + msg + add;
                    }
                    else
                    {
                        incomplete = true;
                        m.name = "**MESSAGE incomplete**";
                    }

                    index++;
                }

                if (!incomplete && c.nameID.Length > 0 && c.hint.Length > 0)
                {
                    c.name = c.nameID + ": " + c.hint;
                }
                else
                {
                    c.name = "**CONVERSATION incomplete**";
                }
            }
        }
	}
}
