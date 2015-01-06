using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[Serializable]
public class LucyExplainingState : BaseState
{
    public bool Skip = false;
    public BaseState NextState;

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
                Debug.LogError("Error: No sound set for LucyExplainingState! Skipping state...");
            }
            AudioObject ao = new AudioObject(script.Lucy, playableSound);
            audioPlayer = AudioManager.PlayAudio(ao);
        }

        // set the player's compass to target lucy
        TargetManager.SetTarget(script.Lucy);
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
    {
        if (audioPlayer != null && !audioPlayer.finished)
            audioPlayer.Delete();
    }
}
