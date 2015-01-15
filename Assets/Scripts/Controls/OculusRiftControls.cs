using System;
using UnityEngine;

[Serializable]
public class OculusRiftControls : BaseControls
{
    public float walkSpeed;
    public float turnSpeed;

    public override void OnEnable()
    {

    }

    public override UnityEngine.Vector3 GetMove(Vector3 current)
    {
        Vector3 forward = CameraManager.GetCameraForwardMovementVector();
        return forward * Time.deltaTime * walkSpeed * Input.GetAxis("Vertical");
    }

    private float GetHorizontalAxis()
    {
        float r = 0f;
        if (Input.GetKey(KeyCode.O))
        {
            r += -1f;
        }
        if (Input.GetKey(KeyCode.P))
        {
            r += 1f;
        }
        return r;
    }

    public override UnityEngine.Quaternion GetRotation(Quaternion current)
    {
        float turned = Time.deltaTime * turnSpeed * GetHorizontalAxis();
        return Quaternion.AngleAxis(turned, Vector3.up);
    }

    public override void OnDisable()
    {

    }
}
