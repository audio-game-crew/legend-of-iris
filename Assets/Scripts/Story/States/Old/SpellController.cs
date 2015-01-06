using UnityEngine;
using System.Collections;
using System;

[Serializable]
public abstract class SpellController : MonoBehaviour
{
	protected MallumController Mallum;

	public float initialSpeed, speed;
	public AudioClip spellSound;

	private AudioPlayer spellPlayer;
	
	public virtual void Init(MallumController m, float startingSpeed) {
		this.Mallum = m;
		this.initialSpeed = startingSpeed;
		this.speed = initialSpeed;
		AudioObject ao = new AudioObject(this.gameObject, spellSound, 1, 0, true);
		spellPlayer = AudioManager.PlayAudio (ao);
	}

	void FixedUpdate() {
		transform.position += transform.forward*((speed)*Time.deltaTime);
	}
	
	protected abstract void OnTriggerEnter (Collider other);
	
	void OnTriggerExit(Collider other) {
		if (other == Mallum.spellLiveArea.collider) {
			Destroy (gameObject);
		}
	}
}

