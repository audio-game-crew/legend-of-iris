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
        timer -= definition.InitialFailTimeout;
        LockOtherRotation(true);
            
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
            convPlayer.ConversationEnd += (s) => { timer = 0; playingConversation = false; };
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
            if (convPlayer != null)
            {
                playingConversation = true;
                convPlayer.Start();
                Reset();
            }
        }

        if (PlayerHasMovedIntoDirection(definition.direction, definition.MovementThreshold))
            Complete();
    }

    private bool PlayerHasMovedIntoDirection(Direction direction, float amount)
    {
        bool sufficientMovement = false;
        switch (direction)
        {
            case Direction.Forward: sufficientMovement = totalMovement.y > 0 && Math.Abs(totalMovement.y) > amount; break;
            case Direction.Backward: sufficientMovement = totalMovement.y < 0 && Math.Abs(totalMovement.y) > amount; break;
            case Direction.Left: sufficientMovement = totalRotation < 0 && Math.Abs(totalRotation) > amount; break;
            case Direction.Right: sufficientMovement = totalRotation > 0 && Math.Abs(totalRotation) > amount; break;
        }
        return sufficientMovement;
    }

    private Direction OppositeDirection(Direction dir)
    {
        switch(dir)
        {
            case Direction.Backward: return Direction.Forward;
            case Direction.Forward: return Direction.Backward;
            case Direction.Left: return Direction.Right;
            case Direction.Right: return Direction.Left;
        }
        return Direction.Backward;
    }

    protected override void _Complete()
    {
        LockOtherRotation(false);
        base._Complete();
    }

    private void LockOtherRotation(bool locked)
    {
        var playerController = Characters.GetPlayerController();
        if (playerController == null) return;
        if (definition.direction == Direction.Backward || definition.direction == Direction.Forward)
        {
            playerController.LockRotation = locked;
            playerController.LockMovement = !locked;
        }
        else
        {
            playerController.LockMovement = locked;
            playerController.LockRotation = !locked;
        }
    }
}
