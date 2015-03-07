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

    public delegate void MessageStartEventHandler(ConversationPlayer player, int index);
    public delegate void MessageEndEventHandler(ConversationPlayer player, int index);
    public delegate void ConversationEndEventHandler(ConversationPlayer player);
    public event MessageStartEventHandler MessageStart;
    public event MessageEndEventHandler MessageEnd;
    public event ConversationEndEventHandler ConversationEnd;
    public int layer;
    private Conversation conversation;
    private List<MessageQueueItem> conversationQueue = new List<MessageQueueItem>();
    private List<AudioPlayer> activePlayers = new List<AudioPlayer>();
    private List<SubtitleElement> activeSubtitles = new List<SubtitleElement>();
    private List<BaseIndicator> activeIndicators = new List<BaseIndicator>();

    private bool conversationEndFired = false;

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
            bool demo = DemoModeManager.instance.demonstrationMode;
            bool skip = demo && m.demoSkip;
            AudioClip clip = demo ? m.audioClipDemo : m.audioClip;

            conversationQueue.Add(new MessageQueueItem()
            {
                message = m,
                index = counter++,
                time = cumulativetime + (skip ? 0f : m.settings.timeOffset)
            });

            if (clip != null)
                cumulativetime += skip ? 0.21f : (clip.length + m.settings.timeOffset);
        }
        conversationQueue = conversationQueue.OrderBy(o => o.time).ToList();
    }

    public void Update()
    {
        var self = this;

        if (conversationQueue.Count <= 0) return;

        if (conversationQueue[0].time > Time.time) return;

        MessageQueueItem mqi = conversationQueue.Shift();
        ConversationMessage msg = mqi.message;
        float timer = msg.audioClip == null ? 0f : msg.audioClip.length;

        bool demo = DemoModeManager.instance.demonstrationMode;
        bool skip = demo && msg.demoSkip;

        // start audio
        AudioClip clip = demo ? msg.audioClipDemo : msg.audioClip;
        AudioPlayer ap = AudioManager.PlayAudio(new AudioObject(msg.source, clip, msg.settings.volume));
        activePlayers.Add(ap);
        ap.SetPitch(msg.settings.pitch);
        ap.SetMinDistance(ConversationManager.instance.minAudioDistance);

        // show indicator
        BaseIndicator ind = null;
        if (msg.settings.screenAsSource)
            ind = IndicatorManager.ShowScreenIndicator(timer);
        else
            ind = IndicatorManager.ShowAudioIndicator(msg.source, timer);
        activeIndicators.Add(ind);

        // set subtitles
        var se = SubtitlesManager.ShowSubtitle(timer, msg.sourceName, msg.subtitle);
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

            if (MessageEnd != null) MessageEnd(self, i);
            if (IsFinished())
            {
                OnConversationEnd();
            }
        });

        // on start
        if (MessageStart != null) MessageStart(self, mqi.index);

        // check if message chould be skipped
        if (skip) ap.MarkRemovable();
    }

    private void OnConversationEnd()
    {
        if (ConversationEnd != null && !conversationEndFired)
        {
            conversationEndFired = true;
            ConversationEnd(this);
        }
    }

    public void Skip()
    {
        var playersCopy = new List<AudioPlayer>(activePlayers);
        // stop all current audio
        foreach (AudioPlayer ap in playersCopy) ap.MarkRemovable();

        // send out all remaning messages and subtitles
        foreach (MessageQueueItem mqi in conversationQueue)
        {
            // set subtitles
            SubtitlesManager.ShowSubtitle(1f, mqi.message.sourceName, mqi.message.subtitle);

            // send out messages
            if (MessageStart != null) MessageStart(this, mqi.index);

            if (MessageEnd != null) MessageEnd(this, mqi.index);
        }

        OnConversationEnd();

        // empty the queue
        conversationQueue = new List<MessageQueueItem>();
    }

    public string GetConversationName()
    {
        return conversation.name;
    }

    public IEnumerable<GameObject> GetConversationActors()
    {
        var actors = new List<GameObject>();
        foreach(var item in conversation.messageSequence)
        {
            var actor = item.source;
            if (!actors.Contains(actor))
                actors.Add(actor);
        }
        return actors;
    }
}
