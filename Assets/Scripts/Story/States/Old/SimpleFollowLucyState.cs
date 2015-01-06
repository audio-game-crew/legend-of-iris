using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// Lucy flies to the target location and emits sounds to guide the player
/// </summary>
[Serializable]
public class SimpleFollowLucyState : LucyRingingBellState
{
    public GameObject TargetLocation;
    public BaseState NextState;

    [Tooltip("Time it takes lucy to fly to the new location")]
    public float LucyAppearanceDelay = 5f;

    private float timeInState = 0f;
    private PositionRotation lucyStart;



    public override void Start(Story script)
    {
        base.Start(script);

        timeInState = 0f;
        lucyStart.Position = script.Lucy.transform.position;
        lucyStart.Rotation = script.Lucy.transform.rotation;
    }

    public override void Update(Story script)
    {
        base.Update(script);

        timeInState += Time.deltaTime;
        float progress = timeInState / LucyAppearanceDelay;
        if (progress < 1)
        {
            var newLucyPos = PositionRotation.Interpolate(lucyStart, 
                new PositionRotation(this.TargetLocation.transform.position, this.TargetLocation.transform.rotation), 
                Ease.ioSinusoidal(progress));
            script.Lucy.transform.position = newLucyPos.Position;
            script.Lucy.transform.rotation = newLucyPos.Rotation;
        }
        else
        {
            script.Lucy.transform.position = TargetLocation.transform.position;
            script.Lucy.transform.rotation = TargetLocation.transform.rotation;
        }

        //Vector3 distance = TargetLocation.transform.position - script.Player.transform.position;
        //// we arrived at the target location, thus load our next state
        //if (distance.magnitude < 2f)
        //{
        //    if (SuccesSound != null)
        //        AudioManager.PlayAudio(new AudioObject(TargetLocation, SuccesSound));
        //    script.LoadState(NextState);
        //}
    }

    public override void PlayerEnteredTrigger(Collider collider, Story script)
    {
        if (collider.gameObject == TargetLocation.gameObject)
        {
            script.PlaySuccessSound(collider.gameObject);
            script.LoadState(NextState);
        }
        base.PlayerEnteredTrigger(collider, script);
    }

    public override void End(Story script)
    {
        base.End(script);
    }
}
