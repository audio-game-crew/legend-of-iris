using UnityEngine;
using System.Collections;

public class RandomFlyScript : MonoBehaviour {

    public float period;
    public Vector3 scale = Vector3.one;
    private float timeOffsetx;
    private float timeOffsety;
    private float timeOffsetz;

	// Use this for initialization
    void Start()
    {
        timeOffsetx = Randomg.Range(0f, 10f);
        timeOffsety = Randomg.Range(0f, 10f);
        timeOffsetz = Randomg.Range(0f, 10f);
	}
	
	// Update is called once per frame
    void Update()
    {
        float tx = Time.time + timeOffsetx;
        float ty = Time.time + timeOffsety;
        float tz = Time.time + timeOffsetz;
        float x = Mathf.Sin(tx * period * 0.9f) + Mathf.Cos(tx * period * 1.4f) + Mathf.Cos(tx * period * 3.6f);
        float y = Mathf.Sin(ty * period * 0.8f) + Mathf.Cos(ty * period * 1.2f) + Mathf.Cos(ty * period * 3.1f);
        float z = Mathf.Sin(tz * period * 0.9f) + Mathf.Cos(tz * period * 1.1f) + Mathf.Cos(tz * period * 3.9f);
        transform.localPosition = new Vector3(x, y, z).times(scale);
	}
}
