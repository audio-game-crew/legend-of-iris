using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CanvasGroup))]
public class Fader : MonoBehaviour {

    CanvasGroup cg;
    public float speed = 1f;
    public bool display = true;

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
        if (display && cg.alpha < 0.999999f)
        {
            cg.alpha = Mathf.Clamp01(cg.alpha + speed * Time.deltaTime);
        }
        else if (!display && cg.alpha > 0.0001f)
        {
            cg.alpha = Mathf.Clamp01(cg.alpha - speed * Time.deltaTime);
            if (cg.alpha < 0.0001f)
            {
                gameObject.SetActive(false);
            }
        }
	}
}
