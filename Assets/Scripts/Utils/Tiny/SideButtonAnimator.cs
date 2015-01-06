using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SideButtonAnimator : MonoBehaviour {

    public float normalSpeed = 1f;
    public float inactiveSpeed = 0.2f;
    public float activeSpeed = 4f;
    public float normalAlpha = 1f;
    public float inactiveAlpha = 0.2f;
    public float magnitude = 0.1f;

    public bool hovering;
    public bool inactive;
    
    private float currentSpeed = 1f;
    private float currentAlpha = 0f;
    private Vector3 startPosition;
    private Image img;
    private float time;

	// Use this for initialization
	void Start () 
    {
        startPosition = transform.position;
        img = GetComponent<Image>();
        time = 0f;
	}

    public void OnHover()
    {
        hovering = true;
    }

    public void OnExit()
    {
        hovering = false;
    }
	
	// Update is called once per frame
	void Update () 
    {
        currentSpeed += ((inactive ? inactiveSpeed : (hovering ? activeSpeed : normalSpeed)) - currentSpeed) * Mathf.Min(1f, Time.unscaledDeltaTime * 5f);
        currentAlpha += ((inactive ? inactiveAlpha : normalAlpha) - currentAlpha) * Mathf.Min(1f, Time.unscaledDeltaTime * 5f);

        time += Time.unscaledDeltaTime * currentSpeed;
        (transform as RectTransform).pivot = new Vector2(0.5f + Mathf.Cos(time) * magnitude, 0.5f);
        img.color = img.color.seta(currentAlpha);
	}
}
