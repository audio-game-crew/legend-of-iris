using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[Serializable]
public class FixedDirectionControls : BaseControls
{
    public bool flipHorizontal = true;
    public bool flipVertical = false;
    [Tooltip("If this boolean is set, the forward direction will be set to the current direction when the player releases the right joystick")]
    public bool ResetForwardRotation = true;
    public float deadZoneSize = 0.5f;
    public float ForwardRotationAngle = 0;
    public Quaternion ForwardRotation { get { return Quaternion.AngleAxis(ForwardRotationAngle, up); } }
    public Vector3 up = new Vector3(0, 1, 0);
    public float turnSpeed = 1f;
    public float moveSpeed = 0.1f;

    private Vector3 lastRotDir;

    private Vector3[] PlaneDirections
    {
        get
        {
            var directions = new Vector3[] { new Vector3(0, 0, 1), new Vector3(0, 1, 0), new Vector3(1, 0, 0) };
            return directions.Select(v => up.cross(v).Positivize().normalized).Where(v => v.magnitude != 0).Distinct().ToArray();
        }
    }

    //private class SameDirectionComparer : IEqualityComparer<Vector3>
    //{

    //    public bool Equals(Vector3 x, Vector3 y)
    //    {
    //        return x.cross(y).magnitude == 0;
    //    }

    //    public int GetHashCode(Vector3 obj)
    //    {
    //        var res = obj.normalized;
    //        if (res.maxDirection() < 0)
    //            return res.flip().GetHashCode();
    //        return res.GetHashCode();
    //    }
    //}

    private Vector3 vert
    {
        get
        {
            var vert = PlaneDirections[0];
            if (flipVertical) vert = vert.flip();
            return vert;
        }
    }

    private Vector3 horz
    {
        get
        {
            var horz = PlaneDirections[1]; 
            if (flipHorizontal) horz = horz.flip();
            return horz;
        }
    }


    public override void OnEnable() 
    {
        lastRotDir = ForwardRotation * vert;
    }

    public override Vector3 GetMove(Vector3 current)
    {
        return GetMoveDir() * moveSpeed;
    }

    private Vector3 GetMoveDir()
    {
        Vector3 forward = CameraManager.GetCameraForwardVector();
        return forward * Time.deltaTime * moveSpeed * Input.GetAxis("Vertical");
        //var vert = PlaneDirections[0]; if (flipVertical) vert = vert.flip();
        //var horz = PlaneDirections[1]; if (flipHorizontal) horz = horz.flip();
        //var moveDir = vert * Input.GetAxis("Vertical") + horz * Input.GetAxis("Horizontal");
        //return moveDir;
    }

    private Vector3 GetLastRotateDir()
    {
        var input = new Vector2(Input.GetAxis("RightH"), Input.GetAxis("RightV"));
        //Debug.Log(input + "(" + input.magnitude + ")");
        if (ResetForwardRotation && input.magnitude < deadZoneSize)
        {
            // Reset the ForwardRotationAngle:
            ForwardRotationAngle = vert.angle(lastRotDir);
        }
        else
        {
            var moveDir = ForwardRotation * (vert * Input.GetAxis("RightV") + horz * Input.GetAxis("RightH"));
            moveDir.Normalize();
            lastRotDir = moveDir;
        }
        return lastRotDir;
    }
    
    public override Quaternion GetRotation(Quaternion current)
    {
        var desiredDir = GetLastRotateDir();
        var currentDir = CameraManager.GetCameraForwardVector();
        var a = currentDir.cross(desiredDir);
        var w = Mathf.Sqrt((Mathf.Pow(currentDir.magnitude, 2) * Mathf.Pow(desiredDir.magnitude, 2)) + currentDir.dot(desiredDir));
        //Debug.Log("w: " + w);
        if (float.IsNaN(w))
            return Quaternion.AngleAxis(180, up);

        var q = new Quaternion(a.x, a.y, a.z, w);
        return q;
    }

    public override void OnDisable() { }
}
