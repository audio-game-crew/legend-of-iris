using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SoundWaveScript : MonoBehaviour {

    public float waveDuration;
    public float waveTimer;
    public AnimationCurve alphaCurve;
    public AnimationCurve scaleCurve;
    private Image r;

	// Use this for initialization
	void Start () {
        r = GetComponent<Image>();
        waveTimer = waveDuration;
        Update();
	}
	
	// Update is called once per frame
	void Update () {
        waveTimer -= Time.deltaTime;
        float t = (waveDuration - waveTimer) / waveDuration;
        r.color = r.color.seta(alphaCurve.Evaluate(t));
        transform.localScale = Vector3.one * scaleCurve.Evaluate(t);
        if (waveTimer < 0f)
        {
            Destroy(gameObject);
        }
	}
}
