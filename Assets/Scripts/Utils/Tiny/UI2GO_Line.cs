using UnityEngine;
using System.Collections;

public class UI2GO_Line : MonoBehaviour {

    public GameObject go;
    public RectTransform ui;
    public float thickness;
    private RectTransform rtrans;

	// Use this for initialization
	void Start () 
    {
        rtrans = GetComponent<RectTransform>();
        UpdateLine();
	}

    void UpdateLine()
    {
        Vector2 from = CameraManager.currentViewingCamera.WorldToScreenPoint(go.transform.position).castxy();
        Vector2 to = ui.anchoredPosition;

        rtrans.anchoredPosition = (from + to) / 2f;
        rtrans.eulerAngles = Vector3.zero.setz(-(from - to).angle(Vector2.up));
        rtrans.sizeDelta = new Vector2(thickness, (from - to).magnitude);
    }
	
	// Update is called once per frame
	void Update () {
        UpdateLine();
	}
}
