using UnityEngine;
using System.Collections;

public class IndicatorManager : MonoBehaviour {

    private static IndicatorManager instance;
    void Awake()
    {
        instance = this;
    }

    public static AudioIndicator ShowAudioIndicator(GameObject source, float timer)
    {
        return instance.showAudioIndicator(source, timer);
    }

    public RectTransform indicatorPanel;
    [Header("Indicator Prefabs")]
    public AudioIndicator audioIndicatorPrefab;

    private AudioIndicator showAudioIndicator(GameObject source, float timer)
    {
        AudioIndicator ai = (AudioIndicator)Instantiate(audioIndicatorPrefab);
        ai.source = source;
        ai.activeTimer = timer;
        RectTransform r = ai.GetComponent<RectTransform>();
        r.SetParent(indicatorPanel);
        r.localPosition = Vector3.zero;
        return ai;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
