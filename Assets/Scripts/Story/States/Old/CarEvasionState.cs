using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


[Serializable]
public class CarEvasionState: SimpleFollowLucyState
{
	public CarGeneratorScript generator;

	// the sound to play, can be attached in unity editor
	public AudioClip carBraking;
	public AudioClip lucyWarning;
	
	// the object that is returned that we listen to, to check if sound is played
	private AudioPlayer audioPlayer;


    public override void Update(Story script)
    {
        base.Update(script);
		// Get all the cars in an array
		var listCars = generator.ListCarsActive;
		var Alex = script.Grumble;
		for (int i=0; i<listCars.Length; i++) {
			var distance = Alex.transform.position - listCars[i].transform.position;
			if (distance.magnitude<5f){
				listCars[i].changeSpeed(0f);
				Debug.Log("Hey you are hitting a car");
				AudioObject ao = new AudioObject(listCars[i].gameObject, carBraking);
				AudioManager.PlayAudio(ao);
				AudioObject ao1 = new AudioObject(script.Lucy.gameObject, lucyWarning);
				AudioManager.PlayAudio(ao1);

			} else {
				listCars[i].changeSpeed(10f);
			}
		}	


    }
}