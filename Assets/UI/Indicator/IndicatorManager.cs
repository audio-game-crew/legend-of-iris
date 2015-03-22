using UnityEngine;
using System.Collections;

public class IndicatorManager : MonoBehaviour {

    private static IndicatorManager instance;
    void Awake()
    {
        instance = this;
    }

    public static BaseIndicator ShowAudioIndicator(GameObject source, float timer, float scale = 1f, float? waveperiod = null, float opacity = 1f)
    {
        return instance.showAudioIndicator(source, timer, scale, waveperiod, opacity);
    }

    public static BaseIndicator ShowScreenIndicator(float timer)
    {
        return instance.showScreenIndicator(timer);
    }

    public RectTransform indicatorPanel;
    [Header("Indicator Prefabs")]
    public SourceAudioIndicator audioIndicatorPrefab;
    public ScreenAudioIndicator screenIndicatorPrefab;

    private BaseIndicator showAudioIndicator(GameObject source, float timer, float scale, float? waveperiod, float opacity)
    {
        SourceAudioIndicator ai = (SourceAudioIndicator)Instantiate(audioIndicatorPrefab);
        ai.transform.localScale = ai.transform.localScale * scale;
        ai.source = source;
        ai.activeTimer = timer;
        if (waveperiod.HasValue)
            ai.wavePeriod = waveperiod.Value;
        RectTransform r = ai.GetComponent<RectTransform>();
        r.SetParent(indicatorPanel, false);
        r.localPosition = Vector3.zero;
        return ai;
    }

    private BaseIndicator showScreenIndicator(float timer)
    {
        ScreenAudioIndicator si = (ScreenAudioIndicator)Instantiate(screenIndicatorPrefab);
        si.activeTimer = timer;
        RectTransform r = si.GetComponent<RectTransform>();
        r.SetParent(indicatorPanel, false);
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
