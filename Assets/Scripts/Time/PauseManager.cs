using UnityEngine;
using System.Collections;

public class PauseManager : MonoBehaviour {

    public static PauseManager instance;
    void Awake()
    {
        instance = this;
    }
    public static bool paused = false;
    public static void Pause()
    {
        if (paused) return;
        paused = true;
        Time.timeScale = 0f;
    }
    public static void Resume()
    {
        if (!paused) return;
        paused = false;
        Time.timeScale = 1f;
    }
    public static void Toggle()
    {
        if (paused) Resume();
        else Pause();
    }
    	
	// Update is called once per frame
	void Update () 
    {
        // only in play mode
        if (!SettingsManager.instance.IsSettingsShown() && Input.GetKeyUp(KeyCode.P))
        {
            Toggle();
        }
	}
}
