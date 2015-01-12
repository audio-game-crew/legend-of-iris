using UnityEngine;
using System.Collections;

public class CheckpointManager : MonoBehaviour {
    private Checkpoint lastCheckpoint;
    public static CheckpointManager instance;

    public AnimationCurve TeleportAnimationVerticalDisplacementValue;
    public AnimationCurve TeleportAnimationHorizontalDisplacementEasing;
    public float JumpHeight = 10;
    public float TeleportTime = 2;

    public Checkpoint InitialCheckpoint;

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
            {
                player.transform.position = target.Position;
                player.transform.rotation = target.Rotation;
                teleporting = false;
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

    public void GotoLastCheckpoint()
    {
        start = new PositionRotation(Characters.instance.Beorn);
        target = new PositionRotation(lastCheckpoint.gameObject);
        teleporting = true;
        time = 0;
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
