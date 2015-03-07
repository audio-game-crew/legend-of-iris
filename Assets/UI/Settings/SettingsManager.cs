using UnityEngine;
using System.Collections;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager instance;
    void Awake()
    {
        instance = this;
    }

    public static int activeSettingType = 0;
    public GameObject settingsBackground;
    public GameObject settingsPanel;
    public float inactiveAlpha = 0.4f;
    public bool pressedUp = false;
    public bool pressedDown = false;
    public AudioPlayer activePlayer;
    public AudioClip latestClip;
    public bool mute = false;

    public void ToggleMute()
    {
        mute = !mute;
        if (activePlayer != null)
        {
            if (mute)
            {
                activePlayer.SetVolume(0f);
            }
            else
            {
                PlaySettingsAudio(latestClip);
            }
        }
    }

    public bool IsSettingsShown()
    {
        return settingsBackground.activeInHierarchy;
    }

    public void ToggleSettings()
    {
        settingsBackground.SetActive(!settingsBackground.activeInHierarchy);
        if (!IsSettingsShown())
        {
            if (activePlayer != null)
                activePlayer.MarkRemovable();
            activeSettingType = 0;
        }
    }

    public void NextSettingType()
    {
        activeSettingType++;
        activeSettingType = Mathf.Clamp(activeSettingType, 0, settingsPanel.transform.childCount - 1);
    }

    public void PreviousSettingType()
    {
        activeSettingType--;
        activeSettingType = Mathf.Clamp(activeSettingType, 0, settingsPanel.transform.childCount - 1);
    }

    public void PlaySettingsAudio(AudioClip ac)
    {
        if (activePlayer != null)
        {
            activePlayer.MarkRemovable();
        }
        AudioObject ao = new AudioObject(ScreenAudioManager.GetScreenAudioObject(), ac, mute ? 0f : 1f, 0f, false, false);
        activePlayer = AudioManager.PlayAudio(ao);
        latestClip = ac;
    }

    void Update()
    {
        if (IsSettingsShown())
        {
            PauseManager.Pause();
            //Screen.showCursor = true;
        }
        else
        {
            PauseManager.Resume();
        }

        if (IsSettingsShown() && Input.GetKeyDown(KeyCode.M))
        {
            ToggleMute();
        }

        float changeSetting = Input.GetAxisRaw("Vertical");
        if (changeSetting < -0.9f) pressedDown = true;
        if (changeSetting > 0.9f) pressedUp = true;
        if (changeSetting > -0.1f && changeSetting < 0.1f)
        {
            if (pressedDown) NextSettingType();
            if (pressedUp) PreviousSettingType();
            pressedDown = false;
            pressedUp = false;
        }

        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.Q))
        {
            Application.Quit();
        }
    }
}
