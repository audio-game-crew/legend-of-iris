using UnityEngine;
using System.Collections;

public class ScreenAudioIndicator : BaseIndicator
{

    public RectTransform screenWavePrefab;
    public float wavePeriod;
    public float activeTimer;
    private float timer;

    public void Stop()
    {
        activeTimer = 0f;
    }

	// Use this for initialization
	void Start () {
        timer = wavePeriod;
        RectTransform r = GetComponent<RectTransform>();
        r.anchoredPosition = Vector2.zero;
        r.sizeDelta = Vector2.zero;
	}
	
	// Update is called once per frame
	void Update () 
    {
        timer += Time.deltaTime;
        activeTimer -= Time.deltaTime;
        if (timer > wavePeriod)
        {
            timer -= wavePeriod;

            if (activeTimer > 0f)
            {
                RectTransform r = (RectTransform)Instantiate(screenWavePrefab);
                r.SetParent(transform);
                r.anchoredPosition = Vector2.zero;
                r.sizeDelta = Vector2.zero;

                //r.localPosition = Vector3.zero;
            }
        }

        if (activeTimer < 0f && transform.childCount == 0)
        {
            Destroy(gameObject);
        }
	}
}
