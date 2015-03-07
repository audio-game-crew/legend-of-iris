using UnityEngine;
using System.Collections;

public class DemoModeManager : MonoBehaviour {

    public static DemoModeManager instance;
    void Awake()
    {
        instance = this;
    }

    public void SetDemonstrationMode()
    {
        demonstrationMode = true;
    }

    public void SetStoryMode()
    {
        demonstrationMode = false;
    }

    public bool demonstrationMode = false;
}
