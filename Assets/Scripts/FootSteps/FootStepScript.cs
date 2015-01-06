using UnityEngine;
using System.Collections.Generic;

public class FootStepScript : MonoBehaviour 
{
    private AudioSource audioSource;
    public List<AudioClip> stepSounds;
    public float maxRandomPitch;

    public void Update()
    {
        // if we are not playing a sound, we finished playing it, so this object can go back into the pool, yay
        if (!audioSource.isPlaying)
        {
            Pool.put(gameObject);
        }
    }

    public void Initialize()
    {
        // remember my audio source component for quick access
        audioSource = GetComponent<AudioSource>();

        // start playing the given sound
        audioSource.clip = stepSounds.GetRandom();
        audioSource.pitch = Randomg.Symmetrical(1f, maxRandomPitch);
        audioSource.Play(); 

        Debug.DrawLine(transform.position, transform.position + Vector3.up, Color.red, audioSource.clip.length);
    }
}
