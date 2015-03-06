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

    public GameObject GameObject
    {
        get { return audioGO; }
    }

    protected AudioObject audio;
    protected GameObject audioGO;
    protected AudioSource audioAS;
    protected AstoundSoundRTIFilter filterAS;
    protected bool paused = false;
    protected float playAtTime = 0f;
    protected bool pausedDueToInactivity = false;

    public delegate void OnRemoveListener();
    protected OnRemoveListener onRemoveListener;

    public AudioPlayer(AudioObject audio)
    {
        if (audio == null || audio.parent == null)
        {
            removable = true;
            return;
        }
        this.finished = false;
        this.removable = false;
        this.paused = false;
        this.audio = audio;

        // create audio source with clip
        audioGO = (GameObject)GameObject.Instantiate(AudioManager.instance.audioSourcePrefab);
        //Debug.Log(audio.parent);
        audioGO.transform.parent = audio.parent.transform;
        audioGO.transform.localPosition = Vector3.zero;

        SoundSystemManager.HandleAudioSource(audioGO);

        audioAS = audioGO.GetComponent<AudioSource>();
        audioAS.Stop();
        audioAS.clip = audio.clip;
        audioAS.volume = audio.volume;
        audioAS.loop = audio.loop;
        if (audio.maxDistance.HasValue)
            audioAS.maxDistance = audio.maxDistance.Value;
        if (audio.minDistance.HasValue)
            audioAS.minDistance = audio.minDistance.Value;

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

    public void PausePlaying(bool forcePausing = false)
    {
        if (audioAS != null && !paused && (forcePausing || audio.pausable))
        {
            paused = true;
            audioAS.Stop();
        }
    }

    public void ContinuePlaying(bool forcePausing = false)
    {
        if (audioAS != null && paused && (forcePausing || audio.pausable) && audioAS.enabled)
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
        if (onRemoveListener != null)
            onRemoveListener.Invoke();
    }

    public void SetVolume(float volume)
    {
        if (audioAS != null)
            audioAS.volume = volume;
    }

    public float GetVolume()
    {
        if (audioAS != null)
            return audioAS.volume;
        return 1f;
    }

    public void SetMinDistance(float distance)
    {
        if (audioAS != null)
            audioAS.minDistance = distance;
    }

    public void SetMaxDistance(float distance)
    {
        if (audioAS != null)
            audioAS.maxDistance = distance;
    }

    public void SetPitch(float pitch)
    {
        if (audioAS != null)
            audioAS.pitch = pitch;
    }

    public void SetTime(float time)
    {
        if (audioAS != null)
            audioAS.time = time;
    }

    public void SetTimePercentage(float percentage)
    {
        if (audioAS != null)
            audioAS.time = GetAudioClip().length * percentage;
    }

    public virtual void Update(float deltaTime)
    {
        if (finished) return;

        if (audioGO != null && !audioGO.activeInHierarchy)
        {
            pausedDueToInactivity = true;
            PausePlaying(true);
        }
        else if (paused && pausedDueToInactivity)
        {
            pausedDueToInactivity = false;
            ContinuePlaying(true);
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

        if (paused)
        {
            playAtTime += deltaTime;
        }

        if (audioAS == null || audioAS.clip == null || (!audioAS.loop && audioAS.time > audioAS.clip.length - 0.04f))
        {
            finished = true;
            MarkRemovable();
        }
    }

    public virtual void OnRemove()
    {
        GameObject.Destroy(audioGO);
    }
}