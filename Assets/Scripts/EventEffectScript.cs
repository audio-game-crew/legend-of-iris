using UnityEngine;
using System.Collections;

public class EventEffectScript : MonoBehaviour {

    public float duration = 1f;
    public float magnitude = 1f;
    public bool animateColor = false;
    private float offset1 = 0f;
    private float offset2 = 0f;
    private float offset3 = 0f;
    // private float offset4 = 0f;
    private Vector3 startScale;

	// Use this for initialization
    void Start()
    {
        startScale = transform.localScale;
        offset1 = Randomg.Range01() * 10f;
        offset2 = Randomg.Range01() * 10f;
        offset3 = Randomg.Range01() * 10f;
        // offset4 = Randomg.Range01() * 10f;
	}

	// Update is called once per frame
    void Update()
    {
        transform.localScale = startScale;
        transform.localScale = transform.localScale.addx(Ease.ioSinusoidal(Ease.loopBounce(Time.time + offset1, duration * 1.3f), - magnitude, 2f * magnitude));
        transform.localScale = transform.localScale.addy(Ease.ioSinusoidal(Ease.loopBounce(Time.time + offset2, duration * 2.0f), - magnitude, 2f * magnitude));
        transform.localScale = transform.localScale.addz(Ease.ioSinusoidal(Ease.loopBounce(Time.time + offset3, duration * 3.1f), - magnitude, 2f * magnitude));
        if (animateColor)
            renderer.material.color = renderer.material.color.seth(Ease.loop(Time.time + offset3, duration * 15f));
	}
}
