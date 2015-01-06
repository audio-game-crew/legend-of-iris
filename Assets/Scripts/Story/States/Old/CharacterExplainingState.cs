using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[Serializable]
public class CharacterExplainingState : BaseState
{
	public bool Skip = false;
	public BaseState NextState;
	public GameObject character;
	
	// the sound to play, can be attached in unity editor
	public AudioClip playableSound;
	
	// the object that is returned that we listen to, to check if sound is played
	private AudioPlayer audioPlayer;
	
	public override void Start(Story script)
	{
		if (!Skip)
		{
			if (playableSound == null)
			{
				Skip = true;
				Debug.LogError("Error: No sound set for CharacterExplainingState! Skipping this state...");
			}
			AudioObject ao = new AudioObject(character, playableSound);
			audioPlayer = AudioManager.PlayAudio(ao);
		}

	}
	
	public override void Update(Story script)
	{
		// wait untill sound is finished, then continue
		if (Skip || audioPlayer.finished)
		{
			script.LoadState(NextState);
		}
	}
	
	public override void End(Story script)
	{ }
}
