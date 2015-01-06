using UnityEngine;
using System.Collections;

public class LucyHoverScript : MonoBehaviour {

    public float duration = 1f;
    public float magnitude = 1f;
    private Vector3 startPosition;

    public GameObject wingLeft;
    public GameObject wingRight;

	// Use this for initialization
	void Start () {
        startPosition = transform.localPosition;
	}
	
	// Update is called once per frame
	void Update ()
    {
        UpdateRotationX();
        UpdateRotationY();
        UpdateRotationZ();
        UpdatePosition();
        UpdateWings();
	}

    void UpdateWings()
    {
        wingRight.transform.localEulerAngles = wingRight.transform.localEulerAngles.setz(Randomg.Range(-20f, 20f));
        wingLeft.transform.localEulerAngles = wingLeft.transform.localEulerAngles.setz(-wingRight.transform.localEulerAngles.z);
    }

    void UpdateRotationX()
    {
        float change = magnitude * 25f;
        transform.localEulerAngles = transform.localEulerAngles.setx(Ease.ioCubic(Ease.loopBounce(Time.time, duration * 1.4f), -change, change + change, 1f));
    }

    void UpdateRotationY()
    {
        float change = magnitude * 25f;
        transform.localEulerAngles = transform.localEulerAngles.sety(Ease.ioCubic(Ease.loopBounce(Time.time, duration * 4.1f), -change, change + change, 1f));
    }

    void UpdateRotationZ()
    {
        float change = magnitude * 25f;
        transform.localEulerAngles = transform.localEulerAngles.setz(Ease.ioCubic(Ease.loopBounce(Time.time, duration * 1.8f), -change, change + change, 1f));
    }

    void UpdatePosition()
    {
        Vector3 change = Vector3.up * magnitude;
        transform.localPosition = Ease.ioCubic(Ease.loopBounce(Time.time), startPosition, change, 1f);
    }
}
