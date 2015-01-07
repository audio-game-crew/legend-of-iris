using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;
using System;
using UnityEngine;


[Serializable]
public class CarGeneratorScript : MonoBehaviour {

	public GameObject car;
    public AudioClip engineSound;
	public GameObject spawnPosition;
	public GameObject carLiveArea;
	public float carSpeed;
	public GameObject street;
	public bool isActive;	
	private Dictionary<GameObject, AudioPlayer> listCarsActive;
	public CarController[] ListCarsActive
	{
		get { return listCarsActive.Select(c => c.Key.GetComponent<CarController>()).ToArray(); }
	}

	private const float MIN_RANDOM_DELAY = 1.0f;
	private const float MAX_RANDOM_DELAY = 7.0f;
	private float timeUntilNextCar;

	// Use this for initialization
	void Start () {
		timeUntilNextCar = 0;
		isActive = true;
        listCarsActive = new Dictionary<GameObject, AudioPlayer>();
	}

	// Update is called once per frame
	void Update () {
		timeUntilNextCar -= Time.deltaTime;

		if (timeUntilNextCar < 0 && isActive) {
			timeUntilNextCar = Randomg.Range(MIN_RANDOM_DELAY, MAX_RANDOM_DELAY);
            SpawnCar();
		}
	}

    private void SpawnCar()
    {
        GameObject carGO = Instantiate(car, spawnPosition.transform.position, spawnPosition.transform.rotation) as GameObject;
        CarController carCS = carGO.GetComponent<CarController>();
        carCS.Init(this, carSpeed);
        carGO.transform.parent = street.transform;
        var player = AudioManager.PlayAudio(new AudioObject(carGO, engineSound, 1, 0, true));
        listCarsActive.Add(carGO, player);
    }

	public void SetActive(bool state) {
		isActive = state;
	}

    public void RemoveCar(GameObject car)
    {
        if (!listCarsActive.ContainsKey(car)) { Debug.LogWarning("Car not found"); return; }
        listCarsActive[car].MarkRemovable();
        listCarsActive.Remove(car);
    }
	/*
	public void changeSpeedCarAt(int i, float speed){
		ListCarsActive.IndexOf (i).changeSpeed (0f);
	}
	*/
}
