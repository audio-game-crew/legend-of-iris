using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CanvasGroup))]
public class Fader : MonoBehaviour {

    CanvasGroup cg;
    public float speed = 1f;
    public bool display = true;
    [Range(0f, 1f)]
    public float minAlpha = 0f;
    [Range(0f, 1f)]
    public float maxAlpha = 1f;

    public void FadeOut()
    {
        display = false;
    }

    public void FadeIn()
    {
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }
        display = true;
    }

	// Use this for initialization
	void Start () {
        cg = GetComponent<CanvasGroup>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (display && cg.alpha < maxAlpha - 0.0001f)
        {
            cg.alpha = Mathf.Clamp(cg.alpha + speed * Time.deltaTime, minAlpha, maxAlpha);
        }
        else if (!display && cg.alpha > minAlpha + 0.0001f)
        {
            cg.alpha = Mathf.Clamp(cg.alpha - speed * Time.deltaTime, minAlpha, maxAlpha);
            if (cg.alpha < 0.0001f)
            {
                gameObject.SetActive(false);
            }
        }
	}
}
