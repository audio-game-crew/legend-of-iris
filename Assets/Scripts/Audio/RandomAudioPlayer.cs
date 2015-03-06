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
        originalVolume = audio.volume;
        originalPitch = audio.pitch;
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
                nextClipStart = Time.time + interval + Random.Range(-intervalVariation, intervalVariation);
                state = State.WAITING;
            }
            break;
        case State.WAITING:
            if (silent) state = State.STOPPING;
            if (Time.time > nextClipStart) {
                audio.clip = GetRandomClip();
                audio.pitch = originalPitch + Random.Range(-pitchVariation, pitchVariation);
                audio.Play();
                state = State.PLAYING;
            }
            break;
        case State.PLAYING:
            if (silent || audio.isPlaying == false) state = State.STOPPING;
            else LerpVolumeTo(originalVolume);
            break;
        }
    }

}
