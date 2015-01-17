using UnityEngine;
using System.Collections;
using System;

public class BorisController : MonoBehaviour {
	[Tooltip("Time it takes Boris to fly to a new location")]
	public float BorisFlyTime = 5f;
	
	public AudioClip BorisSound;
	
	private float flyingTime;
	private PositionRotation BorisStart;
	private PositionRotation targetLocation;
	private bool moving;
	
	private AudioPlayer SoundPlayer;
	private BaseIndicator SoundIndicator;
	private int playing = 0;

    public event EventHandler ArrivedAtLocation;
	
	// Use this for initialization
	void Start () {
		InitSound();
	}
	
	private void InitSound()
	{
		if (BorisSound == null)
			return;
		var audio = new AudioObject(this.gameObject, BorisSound, 1, 0, true, true);
		SoundPlayer = AudioManager.PlayAudio(audio);
		StartSound();
	}
	
	// Update is called once per frame
	void Update () {
		if (moving)
			UpdatePosition();
	}
	
	private void UpdatePosition()
	{
		flyingTime += Time.deltaTime;
		float progress = flyingTime / BorisFlyTime;
		if (progress < 1)
		{
			var newBorisPos = PositionRotation.Interpolate(BorisStart,
			                                              new PositionRotation(this.targetLocation.Position, this.targetLocation.Rotation),
			                                              Ease.ioSinusoidal(progress));
			this.gameObject.transform.position = newBorisPos.Position;
			this.gameObject.transform.rotation = newBorisPos.Rotation;
		}
		else
		{
			this.gameObject.transform.position = targetLocation.Position;
			this.gameObject.transform.rotation = targetLocation.Rotation;
			moving = false;

            if (ArrivedAtLocation != null)
            {
                ArrivedAtLocation(this, EventArgs.Empty);
            }
		}
	}
	
	public void StartSound()
	{
		playing++;
		if (playing > 0)
		{
			if (SoundPlayer != null)
			{
				SoundPlayer.ContinuePlaying();
				SoundPlayer.SetVolume(1);
			}
			if (SoundIndicator != null)
				SoundIndicator.Stop();
			SoundIndicator = IndicatorManager.ShowAudioIndicator(this.gameObject, float.MaxValue, 0.5f);
		}
	}
	
	public void StopBell()
	{
		playing--;
		if (playing <= 0)
		{
			if (SoundIndicator != null)
				SoundIndicator.Stop();
			if (SoundPlayer != null)
			{
				SoundPlayer.PausePlaying();
				SoundPlayer.SetVolume(0);
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
		BorisStart = new PositionRotation(this.gameObject);
		this.targetLocation = loc;
		moving = true;
	}
	
}
