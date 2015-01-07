using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[ExecuteInEditMode]
[RequireComponent(typeof(RectTransform), typeof(CanvasGroup))]
public class SubtitleElement : MonoBehaviour {

    public float inactiveTimerLength = 10f;
    public float activeTimer = 10f;
    public float inactiveTimer = 10f;
    public RectTransform heightDefiner;
    public Text nameField;
    public Text subtitleField;
    public float spacing = 10f;
    public float positionConvergeSpeed = 8f;
    public RectTransform myRectTrans;
    public CanvasGroup myCanvasGroup;
    public int siblingIndex;

    public void Setup(float timer, string name, string subtitle)
    {
        nameField.text = name;
        subtitleField.text = subtitle;
        activeTimer = timer;

        myRectTrans.anchoredPosition = new Vector2(0, -200f);
    }

    public void FadeAfterSeconds(float timer)
    {
        activeTimer = Mathf.Min(activeTimer, timer);
    }

	// Update is called once per frame
	void Update ()
    {
        if (activeTimer > 0f)
        {
            activeTimer -= Time.deltaTime;
            inactiveTimer = inactiveTimerLength;
        }
        else if (inactiveTimer > 0f)
        {
            inactiveTimer -= Time.deltaTime;
            myCanvasGroup.alpha = Mathf.Clamp01(inactiveTimer / inactiveTimerLength);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        Vector2 targetPosition = Vector2.zero;
        siblingIndex = transform.GetSiblingIndex();
        int subtitles = transform.parent.childCount;
        for (int i = subtitles - 1; i > siblingIndex; i--)
        {
            SubtitleElement se = transform.parent.GetChild(i).GetComponent<SubtitleElement>();
            if (se != null)
            {
                targetPosition += new Vector2(0, se.heightDefiner.sizeDelta.y + spacing); 
            }
        }

        if (Application.isEditor && !Application.isPlaying)
            myRectTrans.anchoredPosition = targetPosition;
        else
            myRectTrans.anchoredPosition += (targetPosition - myRectTrans.anchoredPosition) * Timeg.safeDelta(positionConvergeSpeed);
	}
}
