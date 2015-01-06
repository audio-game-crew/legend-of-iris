using System;
using UnityEngine;

[Serializable]
public class SingleAxisControls : BaseControls
{
    public float walkSpeed;
    public float turnSpeed;

    public override void OnEnable()
    {
        
    }

    public override UnityEngine.Vector3 GetMove(Vector3 current)
    {
        Vector3 forward = CameraManager.GetCameraForwardVector().sety(0);
        return forward * Time.deltaTime * walkSpeed * Input.GetAxis("Vertical");
    }

    public override UnityEngine.Quaternion GetRotation(Quaternion current)
    {
        float turned = Time.deltaTime * turnSpeed * Input.GetAxis("Horizontal");
        return Quaternion.AngleAxis(turned, Vector3.up);
    }

    public override void OnDisable()
    {
        
    }
}
