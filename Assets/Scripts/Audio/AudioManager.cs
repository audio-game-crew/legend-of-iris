using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour {

    public static AudioManager instance;

    public static AudioPlayer PlayAudio(AudioObject clip)
    {
        AudioPlayer ap = new AudioPlayer(clip);
        instance.audioPlayers.Add(ap);
        return ap;
    }

    // ---------------- 

    public GameObject audioSourcePrefab;

    private List<AudioPlayer> audioPlayers = new List<AudioPlayer>();
    private void onAudioFinished(AudioPlayer audio)
    {
        audioPlayers.Remove(audio);
    }

    void Awake()
    {
        instance = this;
    }
	
	// Update is called once per frame
	void Update () 
    {
        for (int i = 0; i < audioPlayers.Count; i++)
        {
            AudioPlayer ap = audioPlayers[i];
            ap.Update(Time.deltaTime);
            if (ap.removable)
            {
                ap.OnRemove();
                audioPlayers.Remove(ap);
                i--;
            }
        }
	}
}

