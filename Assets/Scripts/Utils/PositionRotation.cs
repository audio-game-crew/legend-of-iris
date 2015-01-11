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

    public PositionRotation(GameObject source)
    {
        this.Position = source.transform.position;
        this.Rotation = source.transform.rotation;
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

    public static PositionRotation Interpolate(PositionRotation start, PositionRotation end, float progress, AnimationCurve curve)
    {
        var diff = (end.Position - start.Position);
        var curveProgress = curve.Evaluate(progress);
        var pos = new Vector3(
                start.Position.x + diff.x * curveProgress,
                start.Position.y + diff.y * curveProgress,
                start.Position.z + diff.z * curveProgress
                );
        var rot = Quaternion.RotateTowards(start.Rotation, end.Rotation, Quaternion.Angle(start.Rotation, end.Rotation) * curveProgress);
        return new PositionRotation(pos, rot);
    }
}
