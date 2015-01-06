using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System.Collections;
using System;
using System.Collections.Generic;

public class SettingsButton : MonoBehaviour {

    public delegate void ChangedEventHandler();

    [Serializable]
    public class SettingObject {
        public string name;
        [TextArea]
        public string description;
        public AudioClip audio;
        public UnityEvent onSet;
    }

    [Header("Setting")]
    public string name;
    public AudioClip playOnActive;
    public List<SettingObject> settings;

    [Header("State")]
    public int currentSettingIndex = 0;
    private int previousSettingIndex = -1;

    [Header("References")]
    public SideButtonAnimator left;
    public SideButtonAnimator right;
    public Text title;
    public Text settingText;
    public Text descriptionText;

    private bool pressedLeft;
    private bool pressedRight;
    private CanvasGroup canvasGroup;
    private float targetAlpha = 1f;
    private bool previouslyActivated;


    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnHover()
    {
        SettingsManager.activeSettingType = transform.GetSiblingIndex();
    }

    void OnEnable()
    {
        previouslyActivated = false;
    }
	
	// Update is called once per frame
	void Update ()
    {
        bool activated = SettingsManager.activeSettingType == transform.GetSiblingIndex();

        if (activated)
        {
            canvasGroup.alpha += (1f - canvasGroup.alpha) * Mathf.Min(1f, Time.unscaledDeltaTime * 5f);

            float changeSetting = Input.GetAxisRaw("Horizontal");
            if (changeSetting < -0.9f) pressedLeft = true;
            if (changeSetting > 0.9f) pressedRight = true;
            if (changeSetting > -0.1f && changeSetting < 0.1f)
            {
                if (pressedLeft) Left();
                if (pressedRight) Right();
                pressedLeft = false;
                pressedRight = false;
            }
        }

        SettingObject obj = null;
        if (settings.Count > 0)
        {
            obj = settings[currentSettingIndex];
        }

        if (activated && !previouslyActivated && obj != null)
        {
            SettingsManager.instance.PlaySettingsAudio(obj.audio);
        }

        if (activated)
        {
            targetAlpha = 1f;
        }
        else
        {
            targetAlpha = SettingsManager.instance.inactiveAlpha;
        }

        canvasGroup.alpha += (targetAlpha - canvasGroup.alpha) * Mathf.Min(1f, Time.unscaledDeltaTime * 5f);

        if (previousSettingIndex != currentSettingIndex && obj != null)
        {
            if (title != null) title.text = name;
            if (settingText != null) settingText.text = obj.name;
            if (descriptionText != null) descriptionText.text = obj.description;
            if (left != null) left.inactive = !HasLeft();
            if (right != null) right.inactive = !HasRight();

            if (obj.audio != null && activated) SettingsManager.instance.PlaySettingsAudio(obj.audio);
            obj.onSet.Invoke();
        }

        previousSettingIndex = currentSettingIndex;
        previouslyActivated = activated;
	}

    public void Left()
    {
        if (!HasLeft()) return;
        currentSettingIndex--;
        currentSettingIndex = Mathf.Clamp(currentSettingIndex, 0, settings.Count - 1);
    }

    public void Right()
    {
        if (!HasRight()) return;
        currentSettingIndex++;
        currentSettingIndex = Mathf.Clamp(currentSettingIndex, 0, settings.Count - 1);
    }

    public void RightLooped()
    {
        currentSettingIndex++;
        currentSettingIndex = currentSettingIndex % settings.Count;
    }

    public bool HasLeft()
    {
        return currentSettingIndex > 0;
    }

    public bool HasRight()
    {
        return currentSettingIndex < settings.Count - 1;
    }
}
