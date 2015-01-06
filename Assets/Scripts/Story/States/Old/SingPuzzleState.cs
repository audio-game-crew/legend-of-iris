using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[Serializable]
public class SingPuzzleState : BaseState
{
    public BaseState NextState;

	public GameObject Monster1;
	public GameObject Monster2;
	public GameObject Monster3;
	public GameObject Ruby;
	
	// the sound to play, can be attached in unity editor
	public AudioClip playableSoundMonster1;
	public AudioClip playableSoundMonster2;
	public AudioClip playableSoundMonster3;
	public AudioClip playableSoundRuby;
	public AudioClip playableSoundLucyWarning;

	// the object that is returned that we listen to, to check if sound is played
	private AudioPlayer audioPlayer;
	private AudioPlayer monster1Player;
	private AudioPlayer monster2Player;
	private AudioPlayer monster3Player;
	private AudioPlayer rubyPlayer;

	public float MaxRandomDelay = 0.5f;

	public override void Start(Story script)
	{
		monster1Player = PlayWithRandomDelay(Monster1, playableSoundMonster1);
		monster2Player = PlayWithRandomDelay(Monster2, playableSoundMonster2);
		monster3Player = PlayWithRandomDelay(Monster3, playableSoundMonster3);
		rubyPlayer = PlayWithRandomDelay (Ruby, playableSoundRuby);

		base.Start(script);
	}

	public override void Update(Story script)
	{

		// If Alex is close to a monster, scream from the monster and Lucy is warning you to come back !
		// If Alex is close to Rubygo to end
		var distanceMonster1 = script.Grumble.transform.position - Monster1.transform.position;
		var distanceMonster2 = script.Grumble.transform.position - Monster2.transform.position;
		var distanceMonster3 = script.Grumble.transform.position - Monster3.transform.position;
		var distanceRuby = script.Grumble.transform.position - Ruby.transform.position;


		AudioObject ao = new AudioObject(script.Lucy.gameObject, playableSoundLucyWarning);


		if (monster1Player.finished) monster1Player = PlayWithRandomDelay(Monster1, playableSoundMonster1);
		if (monster2Player.finished) monster2Player = PlayWithRandomDelay(Monster2, playableSoundMonster2);
		if (monster3Player.finished) monster3Player = PlayWithRandomDelay(Monster3, playableSoundMonster3);
		if (rubyPlayer.finished) rubyPlayer = PlayWithRandomDelay(Ruby, playableSoundRuby);


		// Best distance to evaluate in order for the game to be nice
		if (distanceMonster1.magnitude < 5.0f) {
			if (audioPlayer.finished)
				AudioManager.PlayAudio(ao);
		} else if (distanceMonster2.magnitude < 5.0f) {
			if (audioPlayer.finished)
				AudioManager.PlayAudio(ao);
		} else if (distanceMonster3.magnitude < 5.0f) {
			if (audioPlayer.finished)
				AudioManager.PlayAudio(ao);
		} else if (distanceRuby.magnitude < 5.0f) {
			script.LoadState(NextState);
		}

		base.Update(script);
	}
	public override void End(Story script)
	{
		base.End(script);
	}

	private AudioPlayer PlayWithRandomDelay(GameObject source, AudioClip clip)
	{
		AudioObject ao = new AudioObject(source, clip, 1, Randomg.Range(0, MaxRandomDelay));
		return AudioManager.PlayAudio(ao);
	}

	
}
