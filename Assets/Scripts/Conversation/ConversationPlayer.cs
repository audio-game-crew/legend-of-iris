using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class ConversationPlayer
{
    private class MessageQueueItem {
        public float time;
        public int index;
        public ConversationMessage message;
    }

    public delegate void OnMessageStart(ConversationPlayer player, int index);
    public delegate void OnMessageEnd(ConversationPlayer player, int index);
    public delegate void OnConversationEnd(ConversationPlayer player);
    public event OnMessageStart onMessageStart;
    public event OnMessageEnd onMessageEnd;
    public event OnConversationEnd onConversationEnd;
    private Conversation conversation;
    private List<MessageQueueItem> conversationQueue = new List<MessageQueueItem>();
    private List<AudioPlayer> activePlayers = new List<AudioPlayer>();
    private List<SubtitleElement> activeSubtitles = new List<SubtitleElement>();
    private List<BaseIndicator> activeIndicators = new List<BaseIndicator>();

    // Make sure the script only disables and enables the bell once
    private bool bellStopped = false;
    private bool bellStarted = false;

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
    }

    public bool IsFinished()
    {
        return RemainingCount == 0 && PlayingCount == 0;
    }

    public void Start()
    {
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

        // Pause Lucy's bell
        SetLucyBellEnabled(false);
        this.onConversationEnd += s => SetLucyBellEnabled(true);
    }

    private void SetLucyBellEnabled(bool enabled)
    {
        //Debug.Log((enabled ? "Enabling" : "Disabling") + " Lucy's bell from " + conversation.name + " " + enabledCount + "," + disabledCount);
        var lucy = Characters.instance.Lucy;
        if (lucy != null)
        {
            var lucyController = lucy.GetComponent<LucyController>();
            if (lucyController != null)
            {
                if (enabled && !bellStarted)
                {
                    lucyController.StartBell();
                    bellStarted = true;
                }
                if (!enabled && !bellStopped)
                {
                    lucyController.StopBell();
                    bellStopped = true;
                }
            }
        }
    }

    public void Update()
    {
        var self = this;

        if (conversationQueue.Count <= 0) return;

        if (conversationQueue[0].time > Time.time) return;

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

            if (onMessageEnd != null) onMessageEnd(self, i);
            if (IsFinished())
            {
                if (onConversationEnd != null) onConversationEnd(self);
            }
        });

        // on start
        if (onMessageStart != null) onMessageStart(self, mqi.index);
    }

    public void Skip()
    {
        var self = this;

        var playersCopy = new List<AudioPlayer>(activePlayers);
        // stop all current audio
        foreach (AudioPlayer ap in playersCopy) ap.MarkRemovable();

        // send out all remaning messages and subtitles
        foreach (MessageQueueItem mqi in conversationQueue)
        {
            // set subtitles
            SubtitlesManager.ShowSubtitle(1f, mqi.message.source.name, mqi.message.subtitle);

            // send out messages
            if (onMessageStart != null) onMessageStart(self, mqi.index);

            if (onMessageEnd != null) onMessageEnd(self, mqi.index);
        }

        if (onConversationEnd != null) onConversationEnd(self);

        // empty the queue
        conversationQueue = new List<MessageQueueItem>();
    }
}
