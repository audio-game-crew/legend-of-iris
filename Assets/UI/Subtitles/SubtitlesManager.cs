using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SubtitlesManager : MonoBehaviour {

    private static SubtitlesManager instance;
    void Awake()
    {
        instance = this;
    }

    public SubtitleElement subtitlePrefab;
    public RectTransform subtitlePanel;

    public static SubtitleElement ShowSubtitle(float timer, string sourceName, string text)
    {
        return instance.showSubtitle(timer, sourceName, text);
    }

    private SubtitleElement showSubtitle(float timer, string sourceName, string text)
    {
        SubtitleElement se = (SubtitleElement) Instantiate(subtitlePrefab);
        se.transform.SetParent(subtitlePanel);
        se.Setup(timer, sourceName.Trim(), text.Trim());
        return se;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
