using UnityEngine;
using System.Collections;
using System;

public class CheckpointManager : MonoBehaviour {
    private Checkpoint lastCheckpoint;
    public static CheckpointManager instance;

    public AnimationCurve TeleportAnimationVerticalDisplacementValue;
    public AnimationCurve TeleportAnimationHorizontalDisplacementEasing;
    public float JumpHeight = 10;
    public float TeleportTime = 2;

    public Checkpoint InitialCheckpoint;

    public event EventHandler StartLastCheckpointTeleport;
    public event EventHandler EndLastCheckpointTeleport;

    private PositionRotation start;
    private PositionRotation target;
    private bool teleporting = false;
    private float time = 0;

    public void Start()
    {
        SetLastCheckpoint(InitialCheckpoint);
    }

    public CheckpointManager()
    {
        instance = this;
    }

	// Update is called once per frame
	void Update () {
        if (teleporting)
        {
            var player = Characters.instance.Beorn;
            time += Time.deltaTime;
            var progress = time / TeleportTime;
            if (progress > 1)
            { // Stop the teleportation
                SetWalkScriptEnabled(true);
                player.transform.position = target.Position;
                player.transform.rotation = target.Rotation;
                teleporting = false;
                OnEndLastCheckpointTeleport();
            }
            else
            {
                var newPosRot = PositionRotation.Interpolate(start, target, progress, TeleportAnimationHorizontalDisplacementEasing);
                player.transform.position = newPosRot.Position
                    // Add the vertical movement to lift up the player a bit while teleporting
                    .addy(TeleportAnimationVerticalDisplacementValue.Evaluate(progress) * JumpHeight);
                player.transform.rotation = newPosRot.Rotation;
            }
        }
	}

    private void SetWalkScriptEnabled(bool enabled)
    {
        var player = Characters.instance.Beorn;
        if (player == null)
            return;

        var walkScript = Characters.instance.Beorn.GetComponent<WalkScript>();
        if (walkScript != null) walkScript.enabled = enabled;
    }

    public void GotoLastCheckpoint(object sender)
    {
        start = new PositionRotation(Characters.instance.Beorn);
        target = new PositionRotation(lastCheckpoint.gameObject);
        teleporting = true;
        SetWalkScriptEnabled(false);
        time = 0;
    }

    public void OnStartLastCheckpointTeleport(object sender)
    {
        if (StartLastCheckpointTeleport != null)
            StartLastCheckpointTeleport(sender, EventArgs.Empty);
    }

    public void OnEndLastCheckpointTeleport()
    {
        if (EndLastCheckpointTeleport != null)
            EndLastCheckpointTeleport(lastCheckpoint, EventArgs.Empty);
    }

    public void SetLastCheckpoint(Checkpoint checkpoint)
    {
        this.lastCheckpoint = checkpoint;
    }

    public Checkpoint GetLastCheckpoint()
    {
        return lastCheckpoint;
    }
}
