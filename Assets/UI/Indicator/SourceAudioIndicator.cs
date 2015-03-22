using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SourceAudioIndicator : BaseIndicator
{

    public GameObject source;
    public RectTransform soundWavePrefab;
    public Vector3 offset;
    public float wavePeriod;
    public float activeTimer;
    private float timer;

    public override void Stop()
    {
        activeTimer = 0f;
        base.Stop();
    }

	// Use this for initialization
	void Start () {
        timer = wavePeriod;
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
                RectTransform r = (RectTransform)Instantiate(soundWavePrefab);
                r.SetParent(transform, false);
                r.localPosition = Vector3.zero;
            }
        }

        if (activeTimer < 0f && transform.childCount == 0)
        {
            Destroy(gameObject);
        }

        var ailo = source.GetComponentInChildren<AudioIndicatorLocationOverride>();
        if (ailo != null) offset = ailo.transform.position - source.transform.position;

        // set position on camera
        Vector3 pos = CameraManager.currentViewingCamera.WorldToScreenPoint(offset + source.transform.position);
        GetComponent<RectTransform>().position = pos.setz(0);
	}
}
