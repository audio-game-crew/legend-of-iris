using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShadowChanger : MonoBehaviour {

    Shadow s;
    public float period;
    public float magnitude;
    public float start;

    void Start()
    {
        s = GetComponent<Shadow>();
    }
	
	// Update is called once per frame
	void Update () 
    {
        Vector2 one = Vector2.one.flipy();
        Vector2 shadow = one * start + one * Mathf.Sin(Time.time / period) * magnitude;
        s.effectDistance = shadow;
        GetComponent<RectTransform>().anchoredPosition = -shadow / 2f;
	}
}
