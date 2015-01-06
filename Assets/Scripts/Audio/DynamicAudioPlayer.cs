using UnityEngine;

public class DynamicAudioPlayer {

    public GameObject gameObject;
    public AudioSource audioSource;

    public DynamicAudioPlayer(GameObject parent, AudioClip clip) {
        gameObject = GameObject.Instantiate(AudioManager.instance.audioSourcePrefab) as GameObject;
        gameObject.transform.SetParent(parent.transform, false);
        audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.clip = clip;
    }

}