using System;
using UnityEngine;

public class MoveQuest : Quest<MoveQuest, MoveQuestDefinition> {

    public Vector3 startPosition;
    public Quaternion startRotation;
    public Vector2 startDirection;

    private bool active = false;

    private Vector2 lastDirection;
    private float totalRotation;

    public MoveQuest(MoveQuestDefinition definition) : base(definition) { }

    protected override void _Start()
    {
        startPosition = Characters.instance.Beorn.transform.position;
        startRotation = Characters.instance.Beorn.transform.rotation;
        startDirection = CameraManager.GetCameraForwardMovementVector().castxz();
        lastDirection = startDirection;
        active = true;
        totalRotation = 0;
	}

    public override void Update()
    {
        if (!active)
            return;
        var player = Characters.instance.Beorn;
        var movement = startRotation * (player.transform.position - startPosition);
        bool sufficientMovement = false;
        
        var curDirection = CameraManager.GetCameraForwardMovementVector().castxz();
        var rotationDelta = lastDirection.angle(curDirection);
        lastDirection = curDirection;

        totalRotation += rotationDelta;
        if (definition.direction == MoveQuestDefinition.Direction.Left || definition.direction == MoveQuestDefinition.Direction.Right)
            Debug.Log(curDirection + " " + lastDirection + " " + rotationDelta + " " + totalRotation);

        switch(definition.direction)
        {
            case MoveQuestDefinition.Direction.Forward: sufficientMovement = movement.z > 0 && Math.Abs(movement.z) > definition.MovementThreshold; break;
            case MoveQuestDefinition.Direction.Backward: sufficientMovement = movement.z < 0 && Math.Abs(movement.z) > definition.MovementThreshold; break;
            case MoveQuestDefinition.Direction.Left: sufficientMovement = totalRotation < 0 && Math.Abs(totalRotation) > definition.MovementThreshold; break;
            case MoveQuestDefinition.Direction.Right: sufficientMovement = totalRotation > 0 && Math.Abs(totalRotation) > definition.MovementThreshold; break;
        }
        if (sufficientMovement)
            Complete();
    }

    protected override void _Complete()
    {
        active = false;
        base._Complete();
    }

	private void Next() {
	}

}
