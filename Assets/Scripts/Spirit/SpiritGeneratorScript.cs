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
	public GameObject spawnPosition;
	public GameObject spiritLiveArea;
	public float spiritSpeed;
	public GameObject crossing;
	public bool isActive;

    public string NearMissConverationID;
    public string HitSpiritConversationID;

    [Tooltip("Ambient Area in which the spirits should be audible.")]
    public AmbientArea SpiritAmbientArea;
    [Tooltip("The name of the Ambient Source containing the sprits of this generator.")]
    public int AmbientSourceID;

    private float conversationCooldown = 0;

    private ConversationPlayer currentConversation;
	private List<GameObject> activeSpirits;
	public SpiritController[] ActiveSpirits
	{
        get { return activeSpirits.Select(s => s.GetComponent<SpiritController>()).ToArray(); }
	}
	
	private const float MIN_RANDOM_DELAY = 1.5f;
	private const float MAX_RANDOM_DELAY = 7.0f;
	private float timeUntilNextSpirit;
	
	// Use this for initialization
	void Start () {
		timeUntilNextSpirit = 0;
		isActive = true;
		activeSpirits = new List<GameObject>();
        conversationCooldown = 0;
	}
	
	// Update is called once per frame
	void Update () {
        conversationCooldown -= Time.deltaTime;
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
        SpiritAmbientArea.AddAmbientLocation(AmbientSourceID, spiritGO);
		activeSpirits.Add(spiritGO);
	}
	
	public void SetActive(bool state) {
		isActive = state;
	}
	
	public void RemoveSpirit(GameObject spirit)
	{

		if (!activeSpirits.Contains(spirit)) { Debug.LogWarning("Spirit not found"); return; }
        SpiritAmbientArea.RemoveAmbientLocation(AmbientSourceID, spirit);
        activeSpirits.Remove(spirit);
	}

    /// <summary>
    /// Call this if the player almost hit a spirit
    /// </summary>
    /// <param name="spirit"></param>
    public void OnNearMissSpirit(GameObject spirit)
    {
        StartConversation(NearMissConverationID, false);
    }

    /// <summary>
    /// Call this if the player hits a spirit
    /// </summary>
    /// <param name="spirit"></param>
    public void OnHitSpirit(GameObject spirit)
    {
        if (currentConversation != null)
            currentConversation.Skip();
        conversationCooldown = 2f;
        CheckpointManager.instance.GotoLastCheckpoint(this.gameObject, HitSpiritConversationID);
    }

    private void StartConversation(string conversationID, bool interruptCurrent)
    {
        if (string.IsNullOrEmpty(conversationID) || conversationCooldown > 0)
            return;
        if (currentConversation != null && !currentConversation.IsFinished())
        {
            if (interruptCurrent)
                currentConversation.Skip();
            else
                return;
        }
        conversationCooldown = 2f;
        currentConversation = ConversationManager.GetConversationPlayer(conversationID);
        currentConversation.Start();
    }
}
