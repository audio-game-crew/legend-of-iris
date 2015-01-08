using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class ConversationPlayer
{
    private class MessageQueueItem
    {
        public float time;
        public int index;
        public ConversationMessage message;
    }

    public delegate void OnMessageStart(int index);
    public delegate void OnMessageEnd(int index);
    private Conversation conversation;
    private List<MessageQueueItem> conversationQueue = new List<MessageQueueItem>();
    private List<AudioPlayer> activePlayers = new List<AudioPlayer>();
    private List<SubtitleElement> activeSubtitles = new List<SubtitleElement>();
    private List<BaseIndicator> activeIndicators = new List<BaseIndicator>();
    private OnMessageStart onMessageStartListener;
    private OnMessageEnd onMessageEndListener;

    public int MessagesCount
    {
        get { return conversation.messageSequence.Count; }
    }

    public int RemainingCount
    {
        get { return conversationQueue.Count; }
    }

    public int PlayingCount
    {
        get { return activePlayers.Count; }
    }

    public ConversationPlayer(Conversation conversation)
    {
        this.conversation = conversation;
        Start();
    }

    public bool IsFinished()
    {
        return RemainingCount == 0 && PlayingCount == 0;
    }

    public void Start(OnMessageStart messageStartListener = null, OnMessageEnd messageEndListener = null)
    {
        onMessageStartListener = messageStartListener;
        onMessageEndListener = messageEndListener;
        conversationQueue = new List<MessageQueueItem>();

        float cumulativetime = Time.time;
        int counter = 0;
        foreach (ConversationMessage m in conversation.messageSequence)
        {
            conversationQueue.Add(new MessageQueueItem()
            {
                message = m,
                index = counter++,
                time = cumulativetime + m.settings.timeOffset
            });

            if (m.audioClip != null)
                cumulativetime += m.audioClip.length + m.settings.timeOffset;
        }
        conversationQueue = conversationQueue.OrderBy(o => o.time).ToList();
    }

    public void Update()
    {
        if (conversationQueue.Count > 0)
        {
            if (conversationQueue[0].time < Time.time)
            {
                MessageQueueItem mqi = conversationQueue.Shift();
                ConversationMessage msg = mqi.message;
                float timer = msg.audioClip == null ? 0f : msg.audioClip.length;

                // start audio
                AudioPlayer ap = AudioManager.PlayAudio(new AudioObject(msg.source, msg.audioClip, msg.settings.volume));
                activePlayers.Add(ap);
                ap.SetPitch(msg.settings.pitch);

                // show indicator
                BaseIndicator ind = null;
                if (msg.settings.screenAsSource)
                    ind = IndicatorManager.ShowScreenIndicator(timer);
                else
                    ind = IndicatorManager.ShowAudioIndicator(msg.source, timer);
                activeIndicators.Add(ind);

                // set subtitles
                var se = SubtitlesManager.ShowSubtitle(timer, msg.source.name, msg.subtitle);
                activeSubtitles.Add(se);

                // on remove
                ap.SetOnRemoveListener(delegate()
                {
                    int i = mqi.index;
                    AudioPlayer myAP = ap;
                    SubtitleElement mySE = se;
                    BaseIndicator myAI = ind;

                    activePlayers.Remove(myAP);
                    activeSubtitles.Remove(mySE);
                    activeIndicators.Remove(myAI);

                    mySE.FadeAfterSeconds(1f);
                    myAI.Stop();

                    if (onMessageEndListener != null)
                    {
                        onMessageEndListener.Invoke(i);
                    }
                });

                // on start
                if (onMessageStartListener != null)
                {
                    onMessageStartListener.Invoke(mqi.index);
                }
            }
        }
    }

    public void Skip()
    {
        // stop all current audio
        foreach (AudioPlayer ap in activePlayers)
        {
            ap.MarkRemovable();
        }

        // send out all remaning messages and subtitles
        foreach (MessageQueueItem mqi in conversationQueue)
        {
            // set subtitles
            SubtitlesManager.ShowSubtitle(1f, mqi.message.source.name, mqi.message.subtitle);

            // send out messages
            if (onMessageStartListener != null)
            {
                onMessageStartListener.Invoke(mqi.index);
            }
            if (onMessageEndListener != null)
            {
                onMessageEndListener.Invoke(mqi.index);
            }
        }

        // empty the queue
        conversationQueue = new List<MessageQueueItem>();
    }
}
