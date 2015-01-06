using UnityEngine;
using System.Collections;

public class ControlsManager : MonoBehaviour {
    public static ControlsManager instance;
    void Awake()
    {
        instance = this;
    }

    public static BaseControls current;
    public SingleAxisControls singleAxisControls;
    public FirstPersonShooterControls firstPersonShooterControls;
    public FixedDirectionControls fixedDirectionControls;
    public ControllerOption DefaultControls = ControllerOption.SingleAxisControls;
    public bool needsReset;

    public void SetSingleAxisControls()
    {
        SetControls(singleAxisControls);
    }

    public void SetFirstPersonShooterControls()
    {
        SetControls(firstPersonShooterControls);
    }

    public void SetFixedDirectionControls()
    {
        SetControls(fixedDirectionControls);
    }

    public bool NeedsReset()
    {
        return needsReset;
    }

    public void ResetDone()
    {
        needsReset = false;
    }

    public void SetControls(BaseControls controls)
    {
        if (current != null)
            current.OnDisable();
        current = controls;
        current.OnEnable();
        needsReset = true;
    }

    void Start()
    {
        switch(DefaultControls)
        {
            case ControllerOption.FixedDirectionControls: SetControls(fixedDirectionControls); break;
            case ControllerOption.SingleAxisControls: SetControls(singleAxisControls); break;
        }
    }

    public enum ControllerOption
    {
        SingleAxisControls, FixedDirectionControls
    }
}
