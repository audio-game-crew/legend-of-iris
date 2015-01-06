using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[Serializable]
public struct PositionRotation
{
    public Vector3 Position { get; set; }
    public Quaternion Rotation { get; set; }

    public PositionRotation(Vector3 position, Quaternion rotation)
    {
        this.Position = position;
        this.Rotation = rotation;
    }

    public static PositionRotation Interpolate(PositionRotation start, PositionRotation end, float progress)
    {
        if (progress <= 0)
            return start;
        if (progress >= 1)
            return end;
        var res = new PositionRotation();
        res.Position = start.Position + ((end.Position - start.Position) * progress);
        res.Rotation = Quaternion.RotateTowards(start.Rotation, end.Rotation, Quaternion.Angle(start.Rotation, end.Rotation) * progress);
        return res;
    }
}
