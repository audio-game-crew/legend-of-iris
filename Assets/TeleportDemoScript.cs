using UnityEngine;
using System.Collections;

public class TeleportDemoScript : MonoBehaviour {

    float time = 0f;
    public GameObject follow;
    public float speed = 2f;
    private Vector3 start;

    public bool useAnimationCurves = false;
    public AnimationCurve verticalDisplacementValue;
    public AnimationCurve horizontalDisplacementEasing;

	// Use this for initialization
	void Start () {
        start = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        time += Time.deltaTime;

        float bounced = Ease.loopBounce(time, speed);
        float bouncedy = Ease.loopBounce(time, speed / 2f);

        Vector3 target = follow.transform.position;
        Vector3 diff = target - start;

        if (useAnimationCurves)
        {
            transform.position = new Vector3(
                start.x + diff.x * horizontalDisplacementEasing.Evaluate(bounced),
                Ease.linear(bounced, start.y, diff.y) + verticalDisplacementValue.Evaluate(bounced),
                start.z + diff.z * horizontalDisplacementEasing.Evaluate(bounced)
                );
        }
        else
        {
            transform.position = new Vector3(
                Ease.ioCubic(bounced, start.x, diff.x),
                Ease.linear(bounced, start.y, diff.y) + Ease.ioCubic(bouncedy, 0f, 10f),
                Ease.ioCubic(bounced, start.z, diff.z)
                );
        }
	}
}
