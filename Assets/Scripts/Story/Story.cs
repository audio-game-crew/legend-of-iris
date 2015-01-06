using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Story : MonoBehaviour {

    [Header("Important objects")]
    public GameObject Grumble;
    public GameObject Lucy;
    public GameObject Boris;
    public GameObject Iris;
    public List<GameObject> Birds;
    [Header("Important sounds")]
    public AudioClip LucyBell;
    public AudioClip SuccessSound;

    /// <summary>
    /// Lucy explains what happened to the player
    /// </summary>
    [Header("Tutorial")]
    public LucyExplainingState LucyExplains1 = new LucyExplainingState();
    /// <summary>
    /// Lucy shows moves a bit to teach the player movement controls
    /// </summary>
    public SimpleFollowLucyState InitialMove = new SimpleFollowLucyState();
    /// <summary>
    /// Lucy moves to the door, inviting the player to open it
    /// </summary>
    public SimpleFollowLucyState SecondMove = new SimpleFollowLucyState();
    public LucyExplainingState LucyExplains2 = new LucyExplainingState();
    /// <summary>
    /// The door is opened when the player moved to it
    /// </summary>
    public LucyRemoveObjectState RemoveDoor = new LucyRemoveObjectState();

    /// <summary>
    /// The player hears the parents talking (screaming) but still has to follow lucy, who is at the next door
    /// </summary>
    [Header("Level 1")]
    public FollowLucyWithTalkingParentsState ParentRoomState = new FollowLucyWithTalkingParentsState();
	public LucyRemoveObjectState RemoveFlatDoor = new LucyRemoveObjectState();

	/// <summary>
	/// Leave the flat and follow lucy to the elevator door
	/// </summary>
	public SimpleFollowLucyState FollowLucyToElevator = new SimpleFollowLucyState ();
    public LucyRemoveObjectState RemoveElevatorDoor = new LucyRemoveObjectState();

    /// <summary>
    /// The player is in the elevator, waiting to get to the street
    /// </summary>
    public TeleportState ElevatorState = new TeleportState();

    /// <summary>
    /// Lucy explains the cars
    /// </summary>
    public LucyExplainingState LucyExplainsCars = new LucyExplainingState();

    /// <summary>
    /// The player has to cross the first road with cars (or something) moving over it.
    /// </summary>
    public CarEvasionState EvadeFirstRoad = new CarEvasionState();

    public StepwiseFollowLucyState WalkPastBuilding1 = new StepwiseFollowLucyState();
    public StepwiseFollowLucyState WalkPastBuilding2 = new StepwiseFollowLucyState();

    /// <summary>
    /// Lucy explains to the player that he/she has to cross some more roads
    /// </summary>
    public LucyExplainingState LucyExplainsToCrossOtherRoad = new LucyExplainingState();
    /// <summary>
    /// The player has to move over to the next position while evading cars
    /// </summary>
    public CarEvasionState EvadeFirstRoadCars = new CarEvasionState();
	public CarEvasionState EvadeSecondRoadCars = new CarEvasionState();

    public StepwiseFollowLucyState WalkToPortal = new StepwiseFollowLucyState();

    public LucyExplainingState LucyExplainsPortal = new LucyExplainingState();
    /// <summary>
    /// The player reached the end of the level and is transported to a magical forest.
    /// </summary>
    public PortalState PortalState = new PortalState();
	public TeleportState teleportLevel2 = new TeleportState();

    [Header("Level 2")]
    public LucyExplainingState ExplainsFortress = new LucyExplainingState();
    public SingPuzzleState SingPuzzle = new SingPuzzleState();
    public CharacterExplainingState RubyExplainsSomething = new CharacterExplainingState();
	public LucyRemoveObjectState RubyRemovesDoorState = new LucyRemoveObjectState();
    public StepwiseFollowLucyState FollowLucyToMines1 = new StepwiseFollowLucyState();
    public StepwiseFollowLucyState FollowLucyToMines2 = new StepwiseFollowLucyState();
	public StepwiseFollowLucyState FollowLucyToMines3 = new StepwiseFollowLucyState();
	public StepwiseFollowLucyState FollowLucyToMines4 = new StepwiseFollowLucyState();
    public MinesPuzzleState MinesPuzzle = new MinesPuzzleState();
	public SimpleFollowLucyState FollowLucyToBoss1 = new SimpleFollowLucyState();
	public SimpleFollowLucyState FollowLucyToBoss2 = new SimpleFollowLucyState();
	public SimpleFollowLucyState FollowLucyToBoss3 = new SimpleFollowLucyState();
    public LucyExplainingState LucyExplainsBoss = new LucyExplainingState();
    public FinalBossState FinalBoss = new FinalBossState();
	public FollowLucyToWaterState FollowLucyToWater = new FollowLucyToWaterState();
    public LucyExplainingState LucyExplainsYouWin = new LucyExplainingState();
    public EndState EndState = new EndState();
 
       
    // Current state that we are in
    private BaseState currentState;
    public Dictionary<GameObject, AudioPlayer> storySoundPlayers = new Dictionary<GameObject, AudioPlayer>();

    // Load the start state
    void Start()
    {
        LoadState(LucyExplains1);
        //LoadState(teleportLevel2);

        // Define for some states that require it what the next state is.
        LucyExplains1.NextState = InitialMove;
        InitialMove.NextState = SecondMove;
        SecondMove.NextState = RemoveDoor;
		// DEBUG LEVEL 2
        //RemoveDoor.NextState = teleportLevel2;
        //teleportLevel2.NextState = ExplainsFortress;
        // Level 1
        RemoveDoor.NextState = ParentRoomState;
        ParentRoomState.NextState = LucyExplains2;
        LucyExplains2.NextState = RemoveFlatDoor;
        RemoveFlatDoor.NextState = FollowLucyToElevator;
        FollowLucyToElevator.NextState = RemoveElevatorDoor;
        RemoveElevatorDoor.NextState = ElevatorState;
        ElevatorState.NextState = LucyExplainsCars;
        LucyExplainsCars.NextState = EvadeFirstRoad;
        EvadeFirstRoad.NextState = WalkPastBuilding1;
        WalkPastBuilding1.NextState = WalkPastBuilding2;
        WalkPastBuilding2.NextState = LucyExplainsToCrossOtherRoad;
        LucyExplainsToCrossOtherRoad.NextState = EvadeFirstRoadCars;
        EvadeFirstRoadCars.NextState = EvadeSecondRoadCars;
        EvadeSecondRoadCars.NextState=WalkToPortal;
        WalkToPortal.NextState = LucyExplainsPortal;
        LucyExplainsPortal.NextState = PortalState;
        PortalState.NextState = teleportLevel2;
        teleportLevel2.NextState = ExplainsFortress;
        // Level 2
        ExplainsFortress.NextState = SingPuzzle;
        SingPuzzle.NextState = RubyExplainsSomething;
		RubyExplainsSomething.NextState = RubyRemovesDoorState;
		RubyRemovesDoorState.NextState=FollowLucyToMines1;
        FollowLucyToMines1.NextState = FollowLucyToMines2;
		FollowLucyToMines2.NextState = FollowLucyToMines3;
		FollowLucyToMines3.NextState = FollowLucyToMines4;
		FollowLucyToMines4.NextState =   MinesPuzzle;
        MinesPuzzle.NextState = FollowLucyToBoss1;
        FollowLucyToBoss1.NextState = FollowLucyToBoss2;
		FollowLucyToBoss2.NextState = FollowLucyToBoss3;
		FollowLucyToBoss3.NextState = LucyExplainsBoss;
        LucyExplainsBoss.NextState = FinalBoss;
        FinalBoss.NextState = LucyExplainsYouWin;
		FinalBoss.PlayerOnFireState = FollowLucyToWater;
		FollowLucyToWater.NextState = FinalBoss;
        LucyExplainsYouWin.NextState = EndState;

        Grumble.GetComponent<PlayerController>().TriggerEntered += OnPlayerEnteredTrigger;
        Grumble.GetComponent<PlayerController>().TriggerExit += OnPlayerExitTrigger;
	}

    void OnPlayerEnteredTrigger(object sender, TriggerEventArgs e)
    {
        currentState.PlayerEnteredTrigger(e.Trigger, this);
    }

    void OnPlayerExitTrigger(object sender, TriggerEventArgs e)
    {
        currentState.PlayerExitTrigger(e.Trigger, this);
    }

	void Update () 
    {
        return;
        RemoveFinishedSuccessSounds();
        if (currentState != null)
            currentState.Update(this);
	}

    public void SetPlayerMovementLocked(bool locked)
    {
        Grumble.GetComponent<PlayerController>().LockMovement = locked;
    }

    public void LoadState(BaseState state)
    {
        if (currentState != null)
            currentState.End(this);

        currentState = state;
        currentState.Start(this);
    }

    private void RemoveFinishedSuccessSounds()
    {
        //Debug.Log(string.Join("; ", storySoundPlayers.Select(s => s.Key.name.ToString() + ", " + s.Value.ToString()).ToArray()));
        List<GameObject> playersToRemove = new List<GameObject>();
        foreach(var go in storySoundPlayers)
        {
            if (go.Value.finished)
            {
                GameObject.Destroy(go.Key);
                playersToRemove.Add(go.Key);
            }
        }
        foreach (var playerToRemove in playersToRemove)
            storySoundPlayers.Remove(playerToRemove);
    }

    public void PlaySuccessSound(GameObject source)
    {
        if (SuccessSound == null) return;
        var go = new GameObject("SuccessSoundPlayer");
        go.transform.parent = source.transform.parent;
        go.transform.position = source.transform.position;
        go.transform.rotation = source.transform.rotation;
        var player = AudioManager.PlayAudio(new AudioObject(go, SuccessSound));
        storySoundPlayers.Add(go, player);
    }
}
