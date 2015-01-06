using System;
using UnityEngine;

[Serializable]
public abstract class BaseControls
{
    public abstract void OnEnable();
    public abstract Vector3 GetMove(Vector3 current);
    public abstract Quaternion GetRotation(Quaternion current);
    public abstract void OnDisable();
}
