using UnityEngine;

public class AudioPlayer
{
    public bool finished
    {
        get;
        protected set;
    }
    public bool removable
    {
        get;
        protected set;
    }

    protected AudioObject audio;
    protected GameObject audioGO;
    protected AudioSource audioAS;
    protected AstoundSoundRTIFilter filterAS;
    protected bool paused = false;
    protected float playAtTime = 0f;

    public delegate void OnRemoveListener();
    protected OnRemoveListener onRemoveListener;

    public AudioPlayer(AudioObject audio)
    {
        this.finished = false;
        this.removable = false;
        this.paused = false;
        this.audio = audio;

        // create audio source with clip
        audioGO = (GameObject)GameObject.Instantiate(AudioManager.instance.audioSourcePrefab);
        audioGO.transform.parent = audio.parent.transform;
        audioGO.transform.localPosition = Vector3.zero;

        SoundSystemManager.HandleAudioSource(audioGO);

        audioAS = audioGO.GetComponent<AudioSource>();
        audioAS.Stop();
        audioAS.clip = audio.clip;
        audioAS.volume = audio.volume;
        audioAS.loop = audio.loop;
        playAtTime = Time.time + audio.delay;
        audioAS.PlayDelayed(audio.delay);
    }

    public void SetOnRemoveListener(OnRemoveListener listener)
    {
        onRemoveListener = listener;
    }

    public AudioClip GetAudioClip()
    {
        if (audioAS != null)
            return audioAS.clip;
        return null;
    }

    public void PausePlaying()
    {
        if (audioAS != null && !paused && audio.pausable)
        {
            paused = true;
            audioAS.Stop();
        }
    }

    public void ContinuePlaying()
    {
        if (audioAS != null && paused && audio.pausable)
        {
            paused = false;

            float time = Time.time - playAtTime;
            if (time > 0)
            {
                audioAS.Play();
                audioAS.time = time;
            }
            else
            {
                audioAS.PlayDelayed(-time);
                audioAS.time = 0;
            }
        }
    }

    public void MarkRemovable()
    {
        removable = true;
    }

    public void SetVolume(float volume)
    {
        if (audioAS != null)
            audioAS.volume = volume;
    }

    public void SetPitch(float pitch)
    {
        if (audioAS != null)
            audioAS.pitch = pitch;
    }

    public virtual void Update(float deltaTime)
    {
        if (finished) return;

        if (paused)
        {
            playAtTime += deltaTime;
        }

        if (audio.pausable)
        {
            if (PauseManager.paused)
            {
                PausePlaying();
            }
            else
            {
                ContinuePlaying();
            }
        }

        if (audioAS == null || audioAS.clip == null || (!audioAS.loop && audioAS.time > audioAS.clip.length - 0.04f))
        {
            finished = true;
            removable = true;
        }
    }

    public virtual void OnRemove()
    {
        GameObject.Destroy(audioGO);

        if (onRemoveListener != null)
            onRemoveListener.Invoke();
    }
}