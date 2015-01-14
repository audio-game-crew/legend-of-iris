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
        audioSourcePrefab = (GameObject)GameObject.Instantiate(audioSourcePrefab); // Make a copy of the prefab
        if (!UnityEditorInternal.InternalEditorUtility.HasPro())
        { // Disable Phonon 3D and Astoundsound on the audio sources, To remove errors for users without pro, 
            var phonon = audioSourcePrefab.GetComponent<BinauralSource>();
            if (phonon != null)
                Component.Destroy(phonon);
            var astoundsound = audioSourcePrefab.GetComponent<AstoundSoundRTIFilter>();
            if (astoundsound != null)
                Component.Destroy(astoundsound);
        }
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

