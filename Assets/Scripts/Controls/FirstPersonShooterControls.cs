using System;
using UnityEngine;

[Serializable]
public class FirstPersonShooterControls : BaseControls
{
    public float walkSpeed;
    public float turnSpeed;
    public bool started = false;
    public float currentY = 0f;
    public float maxY = 30f;

    public override void OnEnable()
    {
        currentY = 0f;
    }

    public override Vector3 GetMove(Vector3 current)
    {
        Vector3 forward = CameraManager.GetCameraForwardVector().sety(0);
        Vector3 right = CameraManager.GetCameraRightVector();
        return (forward * Input.GetAxis("Vertical") + right * Input.GetAxis("Horizontal")) * Time.deltaTime * walkSpeed;
    }

    public override Quaternion GetRotation(Quaternion current)
    {
        float horizontal = Time.deltaTime * turnSpeed * Input.GetAxis("Mouse X");
        float vertical = Time.deltaTime * turnSpeed * -Input.GetAxis("Mouse Y");

        float newY = currentY + vertical;
        newY = Mathf.Clamp(newY, -maxY, maxY);
        vertical = newY - currentY;
        currentY = newY;

        Quaternion hor = Quaternion.AngleAxis(horizontal, Quaternion.Inverse(current) * Vector3.up);
        Quaternion ver = Quaternion.AngleAxis(vertical, Vector3.right);
        return hor * ver;
    }

    public override void OnDisable()
    {
        
    }
}
