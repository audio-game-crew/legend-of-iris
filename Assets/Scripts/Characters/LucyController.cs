using UnityEngine;
using System.Collections;

public class LucyController : MonoBehaviour {
    [Tooltip("Time it takes lucy to fly to a new location")]
    public float LucyFlyTime = 5f;

    private float flyingTime;
    private PositionRotation lucyStart;
    private PositionRotation targetLocation;
    private bool moving;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (moving)
            UpdatePosition();
	}

    private void UpdatePosition()
    {
        flyingTime += Time.deltaTime;
        float progress = flyingTime / LucyFlyTime;
        if (progress < 1)
        {
            var newLucyPos = PositionRotation.Interpolate(lucyStart,
                new PositionRotation(this.targetLocation.Position, this.targetLocation.Rotation),
                Ease.ioSinusoidal(progress));
            this.gameObject.transform.position = newLucyPos.Position;
            this.gameObject.transform.rotation = newLucyPos.Rotation;
        }
        else
        {
            this.gameObject.transform.position = targetLocation.Position;
            this.gameObject.transform.rotation = targetLocation.Rotation;
            moving = false;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(0.3f,0.6f,1f);
        Gizmos.DrawWireSphere(transform.position, 1f);
    }

    public void GotoLocation(PositionRotation loc)
    {
        flyingTime = 0;
        lucyStart = new PositionRotation(this.gameObject);
        this.targetLocation = loc;
        moving = true;
    }

}
