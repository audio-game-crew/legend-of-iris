using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class ButtonRotator : MonoBehaviour {

    public float currentSpeed = 360f;
    public float currentSize = 0.1f;
    public float staticSize = 1f;
    public float staticSpeed = 12f;
    public float activeSpeed = 120f;
    public float activeSize = 1.25f;

    public bool hovering = false;

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
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            ExecuteEvents.Execute(gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.submitHandler);
        }

        currentSpeed += ((hovering ? activeSpeed : staticSpeed) - currentSpeed) * Time.unscaledDeltaTime * 5f;
        currentSize += ((hovering ? activeSize : staticSize) - currentSize) * Time.unscaledDeltaTime * 5f;

        transform.localRotation *= Quaternion.AngleAxis(currentSpeed * Time.unscaledDeltaTime, Vector3.forward);
        transform.localScale = Vector3.one * currentSize;
	}
}
