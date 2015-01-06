using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[Serializable]
public class MinesPuzzleState : SimpleFollowLucyState
{

    public MineFieldScript MineField;
    public GameObject DragBackLocation;
    public AudioClip DragBackSound;
    public GameObject MineFieldDoor;

    private AudioPlayer transportingBackSoundPlayer;
    private float draggingBackTime;
    private bool draggingBack;
    private PositionRotation dragBackStartPosition;
    private PositionRotation dragBackEndPosition;

    public override void Update(Story script)
    {
        if (draggingBack)
            UpdateDragPosition(script.Grumble);
        // Move the player back once the mole has finished dragging.
        if (transportingBackSoundPlayer != null && transportingBackSoundPlayer.finished)
        {
            draggingBack = false;
            MovePlayerBack(script);
            transportingBackSoundPlayer = null;
        }
        base.Update(script);
    }

    private void UpdateDragPosition(GameObject player)
    {
        draggingBackTime += Time.deltaTime;
        float progress = draggingBackTime / DragBackSound.length;
        var newPlayerLoc = PositionRotation.Interpolate(dragBackStartPosition, dragBackEndPosition, progress);
        player.transform.position = newPlayerLoc.Position;
        player.transform.rotation = newPlayerLoc.Rotation;
    }

    private void MovePlayerBack(Story script)
    {
        script.Grumble.transform.position = DragBackLocation.transform.position;
        script.Grumble.transform.rotation = DragBackLocation.transform.rotation;
        script.SetPlayerMovementLocked(false);
    }
    
    public override void PlayerEnteredTrigger(UnityEngine.Collider collider, Story script)
    {
        var collisionType = MineField.GetCollisionType(collider);
        if (collisionType != MineCollisionType.None) // player collided with a mine
        {
            var collidedMine = MineField.GetCollidingMine(collider);
            if (collisionType == MineCollisionType.Proximity)
            {
                MineField.TriggerProximitySensor(collidedMine, true);
            }
            else
            {
                transportingBackSoundPlayer = AudioManager.PlayAudio(new AudioObject(script.Grumble, DragBackSound, 1, 0));
                script.SetPlayerMovementLocked(true);
                MineField.RemoveMine(collidedMine);
                draggingBack = true;
                draggingBackTime = 0f;
                dragBackStartPosition = new PositionRotation(script.Grumble.transform.position, script.Grumble.transform.rotation);
                dragBackEndPosition = new PositionRotation(DragBackLocation.transform.position, DragBackLocation.transform.rotation);
            }
        }
        base.PlayerEnteredTrigger(collider, script);
    }

    public override void PlayerExitTrigger(Collider collider, Story script)
    {
        var collisionType = MineField.GetCollisionType(collider);
        if (collisionType == MineCollisionType.Proximity)
        {
            MineField.TriggerProximitySensor(MineField.GetCollidingMine(collider), false);
        }
        base.PlayerExitTrigger(collider, script);
    }

    public override void End(Story script)
    {
        // TODO: Indicate to the player that the door closes.
        MineFieldDoor.SetActive(true);
        base.End(script);
    } 
}
