using UnityEngine;
using System.Collections;

public class AudioIndicator : MonoBehaviour {

    public GameObject source;
    public RectTransform soundWavePrefab;
    public Vector3 offset;
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
                r.SetParent(transform);
                r.localPosition = Vector3.zero;
            }
        }

        if (activeTimer < 0f && transform.childCount == 0)
        {
            Destroy(gameObject);
        }

        // set position on camera
        Vector3 pos = CameraManager.currentViewingCamera.WorldToScreenPoint(offset + source.transform.position);
        GetComponent<RectTransform>().anchoredPosition = pos;
	}
}
