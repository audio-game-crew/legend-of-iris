using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour {

    public static CameraManager instance;
    void Awake()
    {
        instance = this;
    }

    public VisualAidsCamera visualAidsCamera;
    public Camera oculusRiftRightCamera;
    public Camera normalCamera;
    public CameraSetting setting;
    public Camera current;

    public enum CameraSetting
    {
        NORMAL, HEADTRACKING, OCULUSRIFT
    }

    public static Vector3 GetCameraForwardVector() { return instance.getCameraForwardVector(); }
    private Vector3 getCameraForwardVector()
    {
        return current.transform.TransformDirection(Vector3.forward);
    }

    public static Vector3 GetCameraForwardMovementVector() { return instance.getCameraForwardMovementVector(); }
    private Vector3 getCameraForwardMovementVector()
    {
        return current.transform.TransformDirection(Vector3.forward).sety(0).normalized;
    }

    public static Vector3 GetCameraRightVector() { return instance.getCameraRightVector(); }
    private Vector3 getCameraRightVector()
    {
        return current.transform.TransformDirection(Vector3.right);
    }

    public bool IsOculusRiftConnected()
    {
        try
        {
            if (!OVRDevice.IsHMDPresent())
            {
                return false;
            }
        }
        catch (System.Exception e)
        {
            return false;
        }
        return true;
    }

    public void SetNormalMode()
    {
        setting = CameraSetting.NORMAL;
        setNormalCameraEnabled(true);
        setOculusRiftCameraEnabled(false);
        current = normalCamera;
    }

    public void SetHeadTrackingMode()
    {
        if (!IsOculusRiftConnected())
        {
            Debug.LogError("Oculus Rift is not connected.");
            return;
        }
        setting = CameraSetting.HEADTRACKING;
        setNormalCameraEnabled(true);
        setOculusRiftCameraEnabled(true);
        current = oculusRiftRightCamera;
    }

    public void SetOculusRiftMode()
    {
        if (!IsOculusRiftConnected())
        {
            Debug.LogError("Oculus Rift is not connected.");
            return;
        }
        setting = CameraSetting.OCULUSRIFT;
        setNormalCameraEnabled(false);
        setOculusRiftCameraEnabled(true);
        current = oculusRiftRightCamera;
    }

    private void setNormalCameraEnabled(bool enabled)
    {
        normalCamera.transform.parent.gameObject.SetActive(enabled);
    }

    private void setOculusRiftCameraEnabled(bool enabled)
    {
        oculusRiftRightCamera.transform.parent.gameObject.SetActive(enabled);
    }

    public void HandleSetting()
    {
        switch (setting)
        {
            case CameraSetting.NORMAL:
                SetNormalMode(); break;
            case CameraSetting.HEADTRACKING:
                SetHeadTrackingMode(); break;
            case CameraSetting.OCULUSRIFT:
                SetOculusRiftMode(); break;
        }
    }

    void Start()
    {
        HandleSetting();
    }
}
