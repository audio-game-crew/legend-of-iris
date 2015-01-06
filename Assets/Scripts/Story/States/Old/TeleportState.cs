using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[Serializable]
public class TeleportState : BaseState
{
	public GameObject targetPlayerPosition;
    public GameObject targetLucyPosition;
	public AudioClip sound;
	public BaseState NextState; // TODO pass this to ancestor class?

	private AudioPlayer audioPlayer;

	public override void Start(Story script) {
        script.Grumble.GetComponent<PlayerController>().LockMovement = true;
		AudioObject ao = new AudioObject(script.Lucy, sound);
		audioPlayer = AudioManager.PlayAudio(ao);
	}

	public override void Update(Story script) {
		if (sound == null || audioPlayer.finished) {
			script.LoadState(NextState);
		}
	}

	public override void End(Story script) {
        script.Grumble.GetComponent<PlayerController>().LockMovement = false;
        if (targetPlayerPosition != null)
        {
            script.Grumble.transform.position = targetPlayerPosition.transform.position;
            script.Grumble.transform.rotation = targetPlayerPosition.transform.rotation;
        }
        if (targetLucyPosition != null)
        {
            script.Lucy.transform.position = targetLucyPosition.transform.position;
            script.Lucy.transform.rotation = targetLucyPosition.transform.rotation;
        }
	}
}
