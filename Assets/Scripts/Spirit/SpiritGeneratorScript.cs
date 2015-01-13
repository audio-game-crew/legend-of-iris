using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;
using System;
using UnityEngine;


[Serializable]
public class SpiritGeneratorScript : MonoBehaviour {
	
	public GameObject spirit;
	public AudioClip spiritSound;
	public GameObject spawnPosition;
	public GameObject spiritLiveArea;
	public float spiritSpeed;
	public GameObject crossing;
	public bool isActive;	
	private Dictionary<GameObject, AudioPlayer> listSpiritsActive;
	public SpiritController[] ListSpiritsActive
	{
		get { return listSpiritsActive.Select(c => c.Key.GetComponent<SpiritController>()).ToArray(); }
	}
	
	private const float MIN_RANDOM_DELAY = 1.0f;
	private const float MAX_RANDOM_DELAY = 7.0f;
	private float timeUntilNextSpirit;
	
	// Use this for initialization
	void Start () {
		timeUntilNextSpirit = 0;
		isActive = true;
		listSpiritsActive = new Dictionary<GameObject, AudioPlayer>();
	}
	
	// Update is called once per frame
	void Update () {
		timeUntilNextSpirit -= Time.deltaTime;
		
		if (timeUntilNextSpirit < 0 && isActive) {
			timeUntilNextSpirit = Randomg.Range(MIN_RANDOM_DELAY, MAX_RANDOM_DELAY);
			SpawnSpirit();
		}
	}
	
	private void SpawnSpirit()
	{
		GameObject spiritGO = Instantiate(spirit, spawnPosition.transform.position, spawnPosition.transform.rotation) as GameObject;
		SpiritController spiritCS = spiritGO.GetComponent<SpiritController>();
		spiritCS.Init(this, spiritSpeed);
		spiritGO.transform.parent = crossing.transform;
		var player = AudioManager.PlayAudio(new AudioObject(spiritGO, spiritSound, 1, 0, true));
		listSpiritsActive.Add(spiritGO, player);
	}
	
	public void SetActive(bool state) {
		isActive = state;
	}
	
	public void RemoveSpirit(GameObject spirit)
	{

		Debug.Log ("we call removeSpirit from SpiritGenratorScript");
		if (!listSpiritsActive.ContainsKey(spirit)) { Debug.LogWarning("Spirit not found"); return; }
		//listSpiritsActive[spirit].Delete();
		listSpiritsActive.Remove(spirit);
	}
	/*
	public void changeSpeedSpiritAt(int i, float speed){
		ListSpiritsActive.IndexOf (i).changeSpeed (0f);
	}
	*/
}
