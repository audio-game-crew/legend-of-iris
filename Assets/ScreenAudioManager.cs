using UnityEngine;
using System.Collections;

public class ScreenAudioManager : MonoBehaviour {
    public static ScreenAudioManager instance;
    void Awake()
    {
        instance = this;
    }

    public static void SetScreen(Camera c) 
    {
        instance.screenAudioObject.transform.SetParent(c.transform, false);
    }

    public static GameObject GetScreenAudioObject()
    {
        return instance.screenAudioObject;
    }

    public GameObject screenAudioObject;
}
