using UnityEngine;
using System.Collections;

/*
How to use:
1. Add all Quad and Fence objects to Walls Layer
2. Add PlayerCollision script to alex
3. Attach bump and scathe clips to PlayerCollision script
*/

public class PlayerCollision : MonoBehaviour {

    // the sound to play, can be attached in unity editor
    public AudioClip bumpClip;
    public AudioClip scatheClip;

    // the object that is returned that we listen to, to check if sound is played
    private AudioSource bumpAudioSource;
    private AudioSource scatheAudioSource;
    private float scatheTargetVolume = 0f;
    private int wallLayer;

    void Awake() {
        wallLayer = LayerMask.NameToLayer("Walls");
    }

    void Start() {
        bumpAudioSource = new DynamicAudioPlayer(gameObject, bumpClip).audioSource;
        bumpAudioSource.volume = .8f;
        scatheAudioSource = new DynamicAudioPlayer(gameObject, scatheClip).audioSource;
        scatheAudioSource.loop = true;
    }

    void Update() {
        scatheAudioSource.volume = Mathf.Lerp(scatheAudioSource.volume, scatheTargetVolume, 8*Time.deltaTime);
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.layer != wallLayer) return;
        bumpAudioSource.transform.position = collision.contacts[0].point;
        // prevent playing this too often when colliding between > 1 objects (i.e. two walls)
        if (bumpAudioSource.isPlaying == false) {
            bumpAudioSource.Play();
        }
    }

    void OnCollisionStay(Collision collision) {
        if (collision.gameObject.layer != wallLayer) return;
        scatheAudioSource.transform.position = collision.contacts[0].point;
        scatheTargetVolume = collision.relativeVelocity.magnitude / 1.6f; // just some downscaling
        if (scatheAudioSource.isPlaying == false) {
            scatheAudioSource.Play();
        }
    }

    void OnCollisionExit(Collision collision) {
        if (collision.gameObject.layer != wallLayer) return;
        scatheTargetVolume = 0f;
    }
}