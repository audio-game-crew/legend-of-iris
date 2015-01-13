using System;
using UnityEngine;

public class MoveQuest : Quest<MoveQuest, MoveQuestDefinition> {

    public Vector3 startPosition;
    public Quaternion startRotation;
    public Vector2 startDirection;

    private Vector2 lastDirection;
    private float totalRotation;
    private Vector2 totalMovement;
    
    private float timer;
    private float oppositeDirectionCooldownTimer;
    private bool playingConversation = false;

    public MoveQuest(MoveQuestDefinition definition) : base(definition) { }

    protected override void _Start()
    {
        base._Start();
        Reset();
	}

    private void Reset()
    {
        startPosition = Characters.instance.Beorn.transform.position;
        startRotation = Characters.instance.Beorn.transform.rotation;
        startDirection = CameraManager.GetCameraForwardMovementVector().castxz();
        lastDirection = startDirection;
        totalRotation = 0;
        timer = 0;
        playingConversation = false;
    }

    public override void Update()
    {
        if (state != State.STARTED)
            return;

        // Check if the player failed (e.g. takes to long)
        timer += Time.deltaTime;
        if (!playingConversation && !string.IsNullOrEmpty(definition.FailTimeoutConversation) 
            && timer > definition.FailTimeout)
        {
            var convPlayer = ConversationManager.GetConversationPlayer(definition.FailTimeoutConversation);
            playingConversation = true;
            convPlayer.onConversationEnd += (s) => { timer = 0; playingConversation = false; };
            convPlayer.Start();            
        }

        // Check if the player moved sufficiently to complete the quest
        var player = Characters.instance.Beorn;
        totalMovement = (startRotation * (player.transform.position - startPosition)).castxz();

        
        var curDirection = CameraManager.GetCameraForwardMovementVector().castxz();
        var rotationDelta = lastDirection.angle(curDirection);
        lastDirection = curDirection;

        totalRotation += rotationDelta;
        //if (definition.direction == MoveQuestDefinition.Direction.Left || definition.direction == MoveQuestDefinition.Direction.Right)
        //    Debug.Log(curDirection + " " + lastDirection + " " + rotationDelta + " " + totalRotation);

        if (!playingConversation && !string.IsNullOrEmpty(definition.OppositeDirectionConversation) 
            && PlayerHasMovedIntoDirection(OppositeDirection(definition.direction), definition.OppositeDirectionThreshold))
        {
            var convPlayer = ConversationManager.GetConversationPlayer(definition.OppositeDirectionConversation);
            playingConversation = true;
            convPlayer.onConversationEnd += (s) => { Reset(); };
            convPlayer.Start();
        }

        if (PlayerHasMovedIntoDirection(definition.direction, definition.MovementThreshold))
            Complete();
    }

    private bool PlayerHasMovedIntoDirection(MoveQuestDefinition.Direction direction, float amount)
    {
        bool sufficientMovement = false;
        switch (direction)
        {
            case MoveQuestDefinition.Direction.Forward: sufficientMovement = totalMovement.y > 0 && Math.Abs(totalMovement.y) > amount; break;
            case MoveQuestDefinition.Direction.Backward: sufficientMovement = totalMovement.y < 0 && Math.Abs(totalMovement.y) > amount; break;
            case MoveQuestDefinition.Direction.Left: sufficientMovement = totalRotation < 0 && Math.Abs(totalRotation) > amount; break;
            case MoveQuestDefinition.Direction.Right: sufficientMovement = totalRotation > 0 && Math.Abs(totalRotation) > amount; break;
        }
        return sufficientMovement;
    }

    private MoveQuestDefinition.Direction OppositeDirection(MoveQuestDefinition.Direction dir)
    {
        switch(dir)
        {
            case MoveQuestDefinition.Direction.Backward: return MoveQuestDefinition.Direction.Forward;
            case MoveQuestDefinition.Direction.Forward: return MoveQuestDefinition.Direction.Backward;
            case MoveQuestDefinition.Direction.Left: return MoveQuestDefinition.Direction.Right;
            case MoveQuestDefinition.Direction.Right: return MoveQuestDefinition.Direction.Left;
        }
        return MoveQuestDefinition.Direction.Backward;
    }
}
