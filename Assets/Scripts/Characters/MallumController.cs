using UnityEngine;
using System.Collections;

public class MallumController : MonoBehaviour {

	public GameObject target;
	public GameObject fireball;
	public GameObject powerUp;
	public GameObject spawnPosition;
	public GameObject spellLiveArea;
	public GameObject MallumRoom;
	public float spellSpeed;
	public bool isAwaken;

	private int spellsThrown;
	private int numberHits;
	private bool dead;

	public AudioClip spellSound;
	private AudioPlayer spellPlayer;
	private AudioObject spellAO;

	private const float MIN_RANDOM_DELAY = 3.0f;
	private const float MAX_RANDOM_DELAY = 4.0f;
	private float timeUntilNextFireball;

	// Use this for initialization
	void Start () {
		isAwaken = false;
		spellsThrown = 0;
		timeUntilNextFireball = 0;
		numberHits = 0;
		dead = false;
		spellAO = new AudioObject(this.gameObject, spellSound);
	}
	
	// Update is called once per frame
	void Update () {
		timeUntilNextFireball -= Time.deltaTime;
		
		if (timeUntilNextFireball < 0 && isAwaken) {
			timeUntilNextFireball = Randomg.Range(MIN_RANDOM_DELAY, MAX_RANDOM_DELAY);

			GameObject spellGO = null;
			SpellController spellCS = null;

			if (spellsThrown < 3) {
				spellGO = Instantiate(fireball, spawnPosition.transform.position, spawnPosition.transform.rotation) as GameObject;
				spellCS = spellGO.GetComponent<FireballController>();
				spellsThrown++;
			} else {
				spellGO = Instantiate(powerUp, spawnPosition.transform.position, spawnPosition.transform.rotation) as GameObject;
				spellCS = spellGO.GetComponent<PowerUpController>();
				spellsThrown = 0;
			}

			spellGO.transform.parent = MallumRoom.transform;
			spellGO.transform.LookAt(target.transform);
			spellCS.Init(this, spellSpeed);
			spellPlayer = AudioManager.PlayAudio(spellAO);
		}
	}

	public void getHitByPlayer() {
		numberHits++;
		this.transform.Translate (new Vector3 (3.5f, 0f, 0f));

		if (numberHits >= 2) {
			setAwaken(false);
			dead = true;
		}
	}

	public void setAwaken(bool state) {
		isAwaken = state;
	}

	public bool isDead() {
		return dead;
	}
}
