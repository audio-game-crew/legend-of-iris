using UnityEngine;
using System.Collections.Generic;

[RequireComponent (typeof (AudioSource))]
[RequireComponent (typeof (Phonon3DSource))]

public class RandomAudioPlayer : MonoBehaviour {

    public enum State { STOPPING, WAITING, PLAYING };

    public List<AudioClip> clips = new List<AudioClip>();
    public float interval = 2f;
    public float intervalVariation = 0f;
    public float pitchVariation = 0f;
    public bool silent = false;

    private State state = State.STOPPING;
    private float originalVolume;
    private float originalPitch;
    private float volume;
    private float nextClipStart;

    public void Awake() {
        originalVolume = GetComponent<AudioSource>().volume;
        originalPitch = GetComponent<AudioSource>().pitch;
        clips.RemoveAll(clip => clip == null);
    }

    private void LerpVolumeTo(float target) {
        float v = Mathf.Lerp(GetComponent<AudioSource>().volume, target, Time.deltaTime);
        if (Mathf.Abs(v - target) < 0.01) v = target;
        GetComponent<AudioSource>().volume = v;
        if (GetComponent<AudioSource>().isPlaying && v == 0f) GetComponent<AudioSource>().Stop();
    }

    private AudioClip GetRandomClip() {
        return clips.GetRandom();
    }

    public void Update() {
        switch (state) {
        case State.STOPPING:
            if (GetComponent<AudioSource>().isPlaying) LerpVolumeTo(0f);
            else if (silent == false) {
                nextClipStart = Time.time + interval + Random.Range(-intervalVariation, intervalVariation);
                state = State.WAITING;
            }
            break;
        case State.WAITING:
            if (silent) state = State.STOPPING;
            if (Time.time > nextClipStart) {
                GetComponent<AudioSource>().clip = GetRandomClip();
                GetComponent<AudioSource>().pitch = originalPitch + Random.Range(-pitchVariation, pitchVariation);
                GetComponent<AudioSource>().Play();
                state = State.PLAYING;
            }
            break;
        case State.PLAYING:
            if (silent || GetComponent<AudioSource>().isPlaying == false) state = State.STOPPING;
            else LerpVolumeTo(originalVolume);
            break;
        }
    }

}
