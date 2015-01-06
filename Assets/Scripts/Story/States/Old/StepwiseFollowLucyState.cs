using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[Serializable]
public class StepwiseFollowLucyState : LucyRingingBellState
{
    public BaseState NextState;

    public GameObject TargetLocation;
    public GameObject LucyEventPrefab;

    public float maxStepDistance = 25f;

    [Tooltip("Time it takes lucy to fly to the new location")]
    public float LucyStepFlyTime = 2f;

    private float lucyFlyTime = 0f;
    private PositionRotation LucyLastPosRot;

    private Queue<PositionRotation> steps = new Queue<PositionRotation>();
    private GameObject currentTarget;

    public override void Start(Story script)
    {
        base.Start(script);

        lucyFlyTime = 0f;
        LucyLastPosRot = new PositionRotation(script.Lucy.transform.position, script.Lucy.transform.rotation);

        FillSteps();
        SetNextTarget(script.Lucy);
    }

    private void FillSteps()
    {
        var targetVec = TargetLocation.transform.position - LucyLastPosRot.Position;
        var distance = targetVec.magnitude;
        if (distance < maxStepDistance) // We can just put the final position as the step
            steps.Enqueue(new PositionRotation(TargetLocation.transform.position, TargetLocation.transform.rotation));
        else
        {
            var stepCount = Mathf.Ceil(distance / maxStepDistance);
            var stepSize = 1 / stepCount;
            for (int i = 1; i <= stepCount; i++)
            {
                var percentageDone = stepSize * (float)i;
                Debug.Log(LucyLastPosRot + ", " + TargetLocation);
                steps.Enqueue(new PositionRotation(
                    LucyLastPosRot.Position + percentageDone * targetVec,
                    Quaternion.RotateTowards(LucyLastPosRot.Rotation, this.TargetLocation.transform.rotation, percentageDone
                    )));
            }
        }

    }

    public override void Update(Story script)
    {
        base.Update(script);

        lucyFlyTime += Time.deltaTime;
        float flyProgress = lucyFlyTime / LucyStepFlyTime;

        if (flyProgress < 1)
        {
            script.Lucy.transform.position = LucyLastPosRot.Position + ((this.currentTarget.transform.position - LucyLastPosRot.Position) * Ease.ioSinusoidal(flyProgress));
            script.Lucy.transform.rotation = Quaternion.RotateTowards(LucyLastPosRot.Rotation, this.currentTarget.transform.rotation,
                Quaternion.Angle(LucyLastPosRot.Rotation, this.currentTarget.transform.rotation) * Ease.ioSinusoidal(flyProgress));
        }
        else
        {
            script.Lucy.transform.position = currentTarget.transform.position;
            script.Lucy.transform.rotation = currentTarget.transform.rotation;
        }

    }

    public override void PlayerEnteredTrigger(Collider collider, Story script)
    {
        if (collider.gameObject == currentTarget.gameObject)
        {
            script.PlaySuccessSound(collider.gameObject);
            SetNextTarget(script.Lucy);
        }
        if (collider.gameObject == TargetLocation.gameObject)
        {
            script.PlaySuccessSound(collider.gameObject);
            script.LoadState(NextState);
        }
        base.PlayerEnteredTrigger(collider, script);
    }

    private void SetNextTarget(GameObject lucy)
    {
        if (steps.Any())
        {
            var targetLocRot = steps.Dequeue();
            UnityEngine.Object.Destroy(currentTarget);

            currentTarget = UnityEngine.Object.Instantiate(LucyEventPrefab, targetLocRot.Position, targetLocRot.Rotation) as GameObject;
            SetLucyLastPosRot(lucy);
            lucyFlyTime = 0;
        }
    }

    private void SetLucyLastPosRot(GameObject lucy)
    {
        LucyLastPosRot = new PositionRotation(lucy.transform.position, lucy.transform.rotation);
    }

    public override void End(Story script)
    {
        base.End(script);
    }
}
