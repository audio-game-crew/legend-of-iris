using UnityEngine;
using System.Collections;

public class AmbientManager : MonoBehaviour {
    public static AmbientManager instance;
    void Awake()
    {
        instance = this;
    }

    public static AudioPlayer PlaceAmbientSound(GameObject parent, AudioClip clip, bool loop = true, bool pausable = true)
    {
        AudioObject ao = new AudioObject(parent, clip, 0, 0, loop, pausable);
        return AudioManager.PlayAudio(ao);
    }
}
