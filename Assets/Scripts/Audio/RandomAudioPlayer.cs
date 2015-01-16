using UnityEngine;
using System.Collections.Generic;

[RequireComponent (typeof (AudioSource))]

public class RandomAudioPlayer : MonoBehaviour {

    public enum State { STOPPING, WAITING, PLAYING };

    public List<AudioClip> clips = new List<AudioClip>();
    public float minInterval = 1f;
    public float maxInterval = 3f;
    public bool silent = false;

    private State state = State.STOPPING;
    private float targetVolume;
    private float volume;
    private float nextClipStart;

    public void Awake() {
        targetVolume = audio.volume;
        clips.RemoveAll(clip => clip == null);
    }

    private void LerpVolumeTo(float target) {
        float v = Mathf.Lerp(audio.volume, target, Time.deltaTime);
        if (Mathf.Abs(v - target) < 0.01) v = target;
        audio.volume = v;
        if (audio.isPlaying && v == 0f) audio.Stop();
    }

    private AudioClip GetRandomClip() {
        return clips.GetRandom();
    }

    public void Update() {
        switch (state) {
        case State.STOPPING:
            if (audio.isPlaying) LerpVolumeTo(0f);
            else if (silent == false) {
                nextClipStart = Time.time + Random.Range(minInterval, maxInterval);
                state = State.WAITING;
            }
            break;
        case State.WAITING:
            if (silent) state = State.STOPPING;
            if (Time.time > nextClipStart) {
                audio.clip = GetRandomClip();
                audio.Play();
                state = State.PLAYING;
            }
            break;
        case State.PLAYING:
            if (silent || audio.isPlaying == false) state = State.STOPPING;
            else LerpVolumeTo(targetVolume);
            break;
        }
    }

}
