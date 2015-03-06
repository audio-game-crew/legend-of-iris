using UnityEngine;
using System.Collections;

public class LucyController : MonoBehaviour {
    [Tooltip("Time it takes lucy to fly to a new location")]
    public float LucyFlyTime = 5f;

    public AudioClip LucyBell;

    private float flyingTime;
    private PositionRotation lucyStart;
    private PositionRotation targetLocation;
    private bool moving;

    private AudioPlayer bellPlayer;
    private BaseIndicator bellIndicator;
    private int playing = 0;

	// Use this for initialization
	void Start () {
        InitBell();
	}

    private void InitBell()
    {
        if (LucyBell == null)
            return;
        var audio = new AudioObject(this.gameObject, LucyBell, 1, 0, true, true);
        bellPlayer = AudioManager.PlayAudio(audio);
        playing = 0;
        StopBell();
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

    public void StartBell()
    {
        //Debug.Log("startBell called");
        playing++;
        if (playing >= 0)
        {
            if (bellPlayer != null)
            {
                bellPlayer.ContinuePlaying();
                bellPlayer.SetVolume(1);
            }
            if (bellIndicator != null)
                bellIndicator.Stop();
            bellIndicator = IndicatorManager.ShowAudioIndicator(this.gameObject, float.MaxValue, 0.5f);
        }
    }

    public void StopBell()
    {
        Debug.Log("StopBell called");
        playing--;
        if (playing <= 0)
        {
            if (bellIndicator != null)
                bellIndicator.Stop();
            if (bellPlayer != null)
            {
                bellPlayer.PausePlaying();
                bellPlayer.SetVolume(0);
            }
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

    public void GotoObject(GameObject targetObject) {
        GotoLocation(new PositionRotation(targetObject));
    }

}
