using System;
using System.Collections.Generic;
using UnityEngine;

public class KeyOrderQuest : Quest<KeyOrderQuest, KeyOrderQuestDefinition> {

    private float lastConversationEnd = 0;
    private ConversationPlayer player = null;
    private Queue<Direction> keysToPress;
    private bool firstPlayed = false;

    public KeyOrderQuest(KeyOrderQuestDefinition definition) : base(definition) { }

    protected override void _Start() {
        base._Start();
        Reset();

        SetMovementLocked(true);
        lastConversationEnd = Time.time;

        Debug.Log("KeyOrderQuest started");
    }

    protected override void _Complete()
    {
        SetMovementLocked(false);
        base._Complete();
    }

    private void Reset()
    {
        keysToPress = new Queue<Direction>();
        definition.KeyOrder.ForEach(d => keysToPress.Enqueue(d));
    }

    public override void Update() {
        if (state != State.STARTED)
            return;
        var keyPress = GetKeyPressed();
        if (keyPress == keysToPress.Peek())
        {
            keysToPress.Dequeue();
            if (keysToPress.Count == 0)
                Complete();
        }
        
        if (!string.IsNullOrEmpty(definition.conversationId))
        {
            if (firstPlayed)
            {
                if (Time.time > lastConversationEnd + definition.repeatDelay)
                    StartConversation();
            } else
            {
                if (Time.time > lastConversationEnd + definition.StartConversationFirstDelay)
                    StartConversation();
            }
        }
    }

    private Direction? GetKeyPressed()
    {
        if (Input.GetAxisRaw("Vertical") > 0.9)
            return Direction.Forward;
        if (Input.GetAxisRaw("Vertical") < -0.9)
            return Direction.Backward;
        if (Input.GetAxisRaw("Horizontal") > 0.9)
            return Direction.Right;
        if (Input.GetAxisRaw("Horizontal") < -0.9)
            return Direction.Left;
        return null;
    }

    private void StartConversation() {
        if (player != null) return;
        Debug.Log("Starting conversation yolo, it's been " + (Time.time - lastConversationEnd) + " seconds already");
        player = ConversationManager.GetConversationPlayer(definition.conversationId);
        player.onConversationEnd += OnConversationEnd;
        player.Start();
    }

    private void OnConversationEnd(ConversationPlayer _player) {
        Debug.Log("Fuck this shit, I'm done talking (whoever is talking int eh conveffdfje0re " + _player.GetConversationName());
        _player.onConversationEnd -= OnConversationEnd;
        player = null;
        lastConversationEnd = Time.time;
        firstPlayed = true;
    }

    private void SetMovementLocked(bool locked)
    {
        var player = Characters.GetPlayerController();
        if (player != null)
        {
            player.LockMovement = player.LockRotation = locked;
        }
    }
}
