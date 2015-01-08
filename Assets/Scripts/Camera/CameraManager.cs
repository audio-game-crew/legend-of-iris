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
    public CanvasGroup visualAidsGroup;
    public CameraSetting setting;
    public static Camera currentFirstPersonCamera;
    public static Camera currentViewingCamera;

    public enum CameraSetting
    {
        NORMAL, HEADTRACKING, OCULUSRIFT
    }

    public static Vector3 GetCameraForwardVector() { return instance.getCameraForwardVector(); }
    private Vector3 getCameraForwardVector()
    {
        return currentFirstPersonCamera.transform.TransformDirection(Vector3.forward);
    }

    public static Vector3 GetCameraForwardMovementVector() { return instance.getCameraForwardMovementVector(); }
    private Vector3 getCameraForwardMovementVector()
    {
        return currentFirstPersonCamera.transform.TransformDirection(Vector3.forward).sety(0).normalized;
    }

    public static Vector3 GetCameraRightVector() { return instance.getCameraRightVector(); }
    private Vector3 getCameraRightVector()
    {
        return currentFirstPersonCamera.transform.TransformDirection(Vector3.right);
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
        setVisualAidsCameraEnabled(true);
        currentFirstPersonCamera = normalCamera;
        currentViewingCamera = visualAidsCamera.camera;
        visualAidsGroup.alpha = 1f;
    }

    public void SetHeadTrackingMode()
    {
        if (!IsOculusRiftConnected())
        {
            Debug.LogError("Oculus Rift is not connected.");
            return;
        }
        setting = CameraSetting.HEADTRACKING;
        setNormalCameraEnabled(false);
        setOculusRiftCameraEnabled(true);
        setVisualAidsCameraEnabled(true);
        currentFirstPersonCamera = oculusRiftRightCamera;
        currentViewingCamera = visualAidsCamera.camera;
        visualAidsGroup.alpha = 1f;
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
        setVisualAidsCameraEnabled(false);
        currentFirstPersonCamera = oculusRiftRightCamera;
        currentViewingCamera = oculusRiftRightCamera;
        visualAidsGroup.alpha = 0f;
    }

    private void setNormalCameraEnabled(bool enabled)
    {
        normalCamera.transform.parent.gameObject.SetActive(enabled);
    }

    private void setVisualAidsCameraEnabled(bool enabled)
    {
        visualAidsCamera.gameObject.SetActive(enabled);
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
