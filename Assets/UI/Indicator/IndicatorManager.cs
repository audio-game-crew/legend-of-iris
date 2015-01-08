using UnityEngine;
using System.Collections;

public class IndicatorManager : MonoBehaviour {

    private static IndicatorManager instance;
    void Awake()
    {
        instance = this;
    }

    public static BaseIndicator ShowAudioIndicator(GameObject source, float timer)
    {
        return instance.showAudioIndicator(source, timer);
    }

    public static BaseIndicator ShowScreenIndicator(float timer)
    {
        return instance.showScreenndicator(timer);
    }

    public RectTransform indicatorPanel;
    [Header("Indicator Prefabs")]
    public SourceAudioIndicator audioIndicatorPrefab;
    public ScreenAudioIndicator screenIndicatorPrefab;

    private BaseIndicator showAudioIndicator(GameObject source, float timer)
    {
        SourceAudioIndicator ai = (SourceAudioIndicator)Instantiate(audioIndicatorPrefab);
        ai.source = source;
        ai.activeTimer = timer;
        RectTransform r = ai.GetComponent<RectTransform>();
        r.SetParent(indicatorPanel);
        r.localPosition = Vector3.zero;
        return ai;
    }

    private BaseIndicator showScreenndicator(float timer)
    {
        ScreenAudioIndicator si = (ScreenAudioIndicator)Instantiate(screenIndicatorPrefab);
        si.activeTimer = timer;
        RectTransform r = si.GetComponent<RectTransform>();
        r.SetParent(indicatorPanel);
        r.localPosition = Vector3.zero;
        return si;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
