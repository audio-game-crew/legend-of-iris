using UnityEngine;
using System.Collections;

public class SpiritManager : MonoBehaviour {
	private SpiritGeneratorScript generator;
	public static SpiritManager instance;



	public void Start()
	{

	}
	
	public SpiritManager()
	{
		instance = this;
	}
	
	// Update is called once per frame
	void Update () {
	

	}


}
