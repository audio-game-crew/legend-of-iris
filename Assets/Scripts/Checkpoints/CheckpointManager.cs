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

    public event EventHandler<GotoCheckpointEventArgs> StartLastCheckpointTeleport;
    public event EventHandler<GotoCheckpointEventArgs> EndLastCheckpointTeleport;

    private PositionRotation start;
    private PositionRotation target;
    private bool teleporting = false;
    private float time = 0;
    private ConversationPlayer conversationPlayer;

    public bool IsTeleporting()
    {
        return teleporting;
    }

    public void Start()
    {
        SetLastCheckpoint(InitialCheckpoint);
    }

    public CheckpointManager()
    {
        instance = this;
    }

	// Update is called once per frame
	void FixedUpdate () {
        if (teleporting)
        {
            var player = Characters.instance.Beorn;
            time += Time.deltaTime;
            var progress = time / TeleportTime;
            if (progress > 1)
            { // Stop the teleportation
                player.GetComponent<Rigidbody>().MovePosition(target.Position);
                player.GetComponent<Rigidbody>().useGravity = true;
                player.GetComponent<PlayerController>().camerasContainer.localRotation = target.Rotation;
                teleporting = false;
                if (conversationPlayer == null || conversationPlayer.IsFinished())
                    OnEndLastCheckpointTeleport();
            }
            else
            {
                var newPosRot = PositionRotation.Interpolate(start, target, progress, TeleportAnimationHorizontalDisplacementEasing);
                player.GetComponent<Rigidbody>().MovePosition(newPosRot.Position
                    // Add the vertical movement to lift up the player a bit while teleporting
                    .addy(TeleportAnimationVerticalDisplacementValue.Evaluate(progress) * JumpHeight));
                player.GetComponent<PlayerController>().camerasContainer.localRotation = newPosRot.Rotation;
                //player.transform.rotation = newPosRot.Rotation;
            }
        }
	}

    private void SetMovementRelatedComponentsEnabled(bool enabled)
    {
        var player = Characters.instance.Beorn;
        if (player == null)
            return;

        var walkScript = Characters.instance.Beorn.GetComponent<WalkScript>();
        if (walkScript != null) walkScript.enabled = enabled;

        //var col = Characters.instance.Beorn.collider;
        //if (col != null) col.enabled = enabled;
    }

    private void LockMovement(bool enabled)
    {
        var player = Characters.instance.Beorn;
        if (player == null)
            return;

        var playerController = player.GetComponent<PlayerController>();
        playerController.LockMovement = enabled;
    }

    public void GotoLastCheckpoint(object sender, string conversation = null)
    {
        TimerManager.RegisterEvent("Dieded");
        var player = Characters.instance.Beorn;
        start = new PositionRotation(player.transform.position, Characters.GetPlayerController().camerasContainer.localRotation);
        target = new PositionRotation(lastCheckpoint.gameObject);
        player.GetComponent<Rigidbody>().useGravity = false;
        teleporting = true;
        SetMovementRelatedComponentsEnabled(false);
        time = 0;
        OnStartLastCheckpointTeleport(sender, conversation);
        if (conversation != null)
        {
            PlayConversation(conversation);
        }
    }

    private void PlayConversation(string conversation)
    {
        var player = ConversationManager.GetConversationPlayer(conversation);
        player.ConversationEnd += s => OnEndLastCheckpointTeleport();
        player.Start();
    }

    public void OnStartLastCheckpointTeleport(object sender, string conversation)
    {
        SetMovementRelatedComponentsEnabled(false);
        LockMovement(true);
        if (StartLastCheckpointTeleport != null)
            StartLastCheckpointTeleport(sender, new GotoCheckpointEventArgs(conversation));
    }

    public void OnEndLastCheckpointTeleport()
    {
        SetMovementRelatedComponentsEnabled(true);
        LockMovement(false);
        if (EndLastCheckpointTeleport != null)
            EndLastCheckpointTeleport(lastCheckpoint, new GotoCheckpointEventArgs());
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
