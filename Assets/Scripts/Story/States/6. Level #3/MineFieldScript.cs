using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MineFieldScript : MonoBehaviour {
    [Header("MineField grid properties")]
    public int SizeX = 10;
    public int SizeY = 10;
    public int MineCount = 7;
    public float GridCellSize = 2f;
    public int MaxPlacementTries = 1000;
    public Point2 StartPos = new Point2();
    public Point2 EndPos = new Point2(10, 10);
    [Header("Appeareance properties")]
    public GameObject Mine;
    public GameObject MineProximitySensor;
    [Header("Sounds")]
    public AudioClip MineSound;
    public AmbientArea MineSoundArea = null;
    public float SoundMinDistance = 0.2f;

    [Header("Minefield types")]
    public bool OnlyProximity  = false;
    public bool CheckPath = true;
    public bool MarkAll = false;

    [Header("Gameplay details")]
    public Checkpoint DragBackCheckpoint;
    public List<string> FailConversations = new List<string> { "T6.2" };

    public GameObject[] Mines { get { return mines.ToArray(); } }
    private bool UseAmbient { get { return !OnlyProximity && MineSoundArea != null; } }

    private bool[,] grid;
    private List<GameObject> mines = new List<GameObject>();
    private Dictionary<GameObject, GameObject> mineProximitySensors = new Dictionary<GameObject, GameObject>();
    private Dictionary<GameObject, AudioPlayer> minePlayers = new Dictionary<GameObject, AudioPlayer>();
    private Dictionary<GameObject, AmbientArea.AmbientSource> mineAmbientPlayers = new Dictionary<GameObject, AmbientArea.AmbientSource>();
    private Dictionary<GameObject, Point2> minePositions = new Dictionary<GameObject, Point2>();
    private int AmbientSoundInitialCount = 0;
    private int placementTries = 0;
    private bool initialized = false;

	// Use this for initialization
	void Start () 
    {
        if (OutsideGrid(StartPos))
            Debug.LogWarning("Start Position is outside the grid");
        if (OutsideGrid(EndPos))
            Debug.LogWarning("End Position is outside the grid");
        initialized = false;
        if (MineSoundArea != null)
            AmbientSoundInitialCount = MineSoundArea.ambientSources.Count;
	}

    private void Init()
    {
        grid = new bool[SizeX, SizeY];
        placementTries = 0;
        PlaceMines();
        AddMines();
        if (CheckpointManager.instance != null)
            CheckpointManager.instance.SetLastCheckpoint(DragBackCheckpoint);
    }

    void Update()
    {
        if (!initialized)
        {
            try
            {
                Init();
            } catch
            {
                mines.ForEach(m => RemoveMine(m));
            }
            initialized = true;
        }

    }

    void OnEnable()
    {
        // Register collider events
        var player = Characters.instance.Beorn.GetComponent<PlayerController>();
        player.TriggerEntered += player_TriggerEntered;
        player.TriggerExit += player_TriggerExit;
    }

    void OnDisable()
    {
        // Unregister collider events
        var player = Characters.instance.Beorn.GetComponent<PlayerController>();
        player.TriggerEntered -= player_TriggerEntered;
        player.TriggerExit -= player_TriggerExit;
    }

    private void ResetGrid()
    {
        for (int i = 0; i < SizeX; i++)
            for (int j = 0; j < SizeY; j++)
            {
                grid[i, j] = MarkAll;
            }
    }

    private bool OutsideGrid(Point2 pos)
    {
        return pos.x < 0 || pos.x >= SizeX || pos.y < 0 || pos.y >= SizeY;
    }

    private void PlaceMines()
    {
        placementTries++;
        ResetGrid();
        if (!MarkAll)
        {
            int placedMines = 0;
            while (placedMines < MineCount)
            {
                int x = Random.Range(0, SizeX);
                int y = Random.Range(0, SizeY);
                if (!grid[x, y] && !(StartPos.x == x && StartPos.y == y) && !(EndPos.x == x && EndPos.y == y))
                {
                    grid[x, y] = true;
                    placedMines++;
                }
            }
            if (CheckPath && !PathExists() && placementTries < MaxPlacementTries)
                PlaceMines();
        }
    }

    private bool PathExists()
    {
        var reachables = new List<Point2>();
        reachables.Add(StartPos);
        int marked = 1;
        while (marked != 0)
        {
            int reachableCount = reachables.Count;
            foreach(var reachable in reachables.ToArray())
            {
                var neighbors = new Point2[] { 
                    new Point2(reachable.x - 1, reachable.y),
                    new Point2(reachable.x + 1, reachable.y),
                    new Point2(reachable.x, reachable.y + 1),
                    new Point2(reachable.x, reachable.y - 1)
                };
                foreach(var neighbor in neighbors)
                {
                    if (!OutsideGrid(neighbor) && !grid[neighbor.x, neighbor.y] && !reachables.Contains(neighbor))
                        reachables.Add(neighbor);
                }
            }
            marked = reachables.Count - reachableCount;
        }
        return reachables.Contains(EndPos);
    }

    private void AddMines()
    {
        // Clear the old mines
        foreach (var mineProximitySensor in mineProximitySensors.Values)
            GameObject.Destroy(mineProximitySensor);
        foreach (var minePlayer in minePlayers.Values)
            minePlayer.MarkRemovable();
        foreach (var mineAmbientPlayer in mineAmbientPlayers.Values)
            MineSoundArea.ambientSources.Remove(mineAmbientPlayer);
        foreach (var mine in mines)
            GameObject.Destroy(mine);
        mineProximitySensors.Clear();
        minePlayers.Clear();
        mineAmbientPlayers.Clear();
        mines.Clear();

        // Add the current mines
        for(int i = 0; i < SizeX; i++)
            for (int j = 0; j < SizeY; j++)
            {
                if (grid[i, j])
                {
                    AddMine(i, j);
                }
            }
    }

    private void AddMine(int i, int j)
    {
        var minePosition = this.transform.position + GetRelativeMinePosition(i, j);
        var mine = (GameObject)GameObject.Instantiate(Mine, minePosition, new Quaternion());
        mine.transform.parent = this.transform;
        var delay = Randomg.Range(0f, MineSound.length);
        if (UseAmbient)
        {
            var selectedAmbientMineSound = MineSoundArea.ambientSources[Random.Range(0, AmbientSoundInitialCount)];
            var copy = (AmbientArea.AmbientSource)selectedAmbientMineSound.Clone();
            copy.locations[0] = mine;
            MineSoundArea.ambientSources.Add(copy);
            mineAmbientPlayers.Add(mine, copy);
        }
        else
        {
            var minePlayer = AudioManager.PlayAudio(new AudioObject(mine, MineSound, OnlyProximity ? 0 : 1, delay, true) { minDistance = SoundMinDistance });
            minePlayers.Add(mine, minePlayer);
        }
        if (MineProximitySensor != null)
        {
            var mineProximitySensor = (GameObject)GameObject.Instantiate(MineProximitySensor, minePosition, new Quaternion());
            mineProximitySensor.transform.parent = this.transform;
            mineProximitySensors.Add(mine, mineProximitySensor);
        }
        mines.Add(mine);
        
        minePositions.Add(mine, new Point2(i, j));
    }

    #region HandlePlayerCollisions
    void player_TriggerExit(object sender, TriggerEventArgs e)
    {
        var collider = e.Trigger;
        var collisionType = GetCollisionType(collider);
        if (collisionType == MineCollisionType.Proximity)
        {
            TriggerProximitySensor(GetCollidingMine(collider), false);
        }
    }

    void player_TriggerEntered(object sender, TriggerEventArgs e)
    {
        var collider = e.Trigger;
        var collisionType = GetCollisionType(collider);
        if (collisionType != MineCollisionType.None) // player collided with a mine or proximity sensor
        {
            var collidedMine = GetCollidingMine(collider);
            if (collisionType == MineCollisionType.Proximity)
            {
                TriggerProximitySensor(collidedMine, true);
            }
            else
            { // Player collided with a mine.
                var failPlayer = ConversationManager.GetConversationPlayer(FailConversations[Random.Range(0, FailConversations.Count)]);
                failPlayer.ConversationEnd += failPlayer_onConversationEnd;
                failPlayer.Start();
                RemoveMine(collidedMine);
            }
        }
    }

    void failPlayer_onConversationEnd(ConversationPlayer player)
    {
        player.ConversationEnd -= failPlayer_onConversationEnd;
        CheckpointManager.instance.GotoLastCheckpoint(this);
    }

    public MineCollisionType GetCollisionType(Collider col)
    { 
        var colGO = col.gameObject;
        if (mineProximitySensors.Any(m => m.Value == colGO))
            return MineCollisionType.Proximity;
        if (mines.Any(m => m == colGO))
            return MineCollisionType.Mine;
        return MineCollisionType.None;
    }

    public GameObject GetCollidingMine(Collider col)
    {
        if (GetCollisionType(col) == MineCollisionType.None)
            return null;
        return mines.FirstOrDefault(m => m == col.gameObject) ?? mineProximitySensors.First(m => m.Value == col.gameObject).Key;
    }

    public void TriggerProximitySensor(GameObject mine, bool trigger)
    {
        Debug.Log("Setting Proximity sensor to " + trigger);
        if (minePlayers.ContainsKey(mine))
        {
            if (OnlyProximity)
                minePlayers[mine].SetVolume(trigger ? 1 : 0);
            minePlayers[mine].SetPitch(trigger ? 2 : 1);
        } else if (mineAmbientPlayers.ContainsKey(mine))
        {
            mineAmbientPlayers[mine].minPitch = trigger ? 1.3f : 0.9f;
            mineAmbientPlayers[mine].maxPitch = trigger ? 1.5f : 1.1f;
        }

    }
    #endregion

    public void RemoveMine(GameObject mineToRemove)
    {
        if (!mines.Contains(mineToRemove))
            return;
        Debug.Log("Removing mine " + mineToRemove.name, mineToRemove);
        if (mineProximitySensors.ContainsKey(mineToRemove))
            mineProximitySensors[mineToRemove].SetActive(false);
        if (mineAmbientPlayers.ContainsKey(mineToRemove))
            MineSoundArea.ambientSources.Remove(mineAmbientPlayers[mineToRemove]);
        if (minePlayers.ContainsKey(mineToRemove))
            minePlayers[mineToRemove].MarkRemovable();
        mineToRemove.SetActive(false);
        var minePosition = minePositions[mineToRemove];
        grid[minePosition.x, minePosition.y] = false;
    }

    void OnDrawGizmos()
    {
        Vector3 cellSize = transform.localScale.divide(new Vector3(SizeX, 1, SizeY));
        for(int i = 0; i < SizeX; i++)
            for (int j = 0; j < SizeY; j++)
            {
                Gizmos.color = grid != null && grid[i, j] ? Color.red : Color.cyan;
                if (StartPos.x == i && StartPos.y == j)
                    Gizmos.color = Color.green;
                if (EndPos.x == i && EndPos.y == j)
                    Gizmos.color = Color.magenta;
                Gizmos.DrawWireCube(
                    transform.position + GetRelativeMinePosition(i, j),
                    cellSize);

            }
    }

    private Vector3 GetRelativeMinePosition(Point2 pos)
    {
        return GetRelativeMinePosition(pos.x, pos.y);
    }

    private Vector3 GetRelativeMinePosition(int i, int j)
    {
        Vector3 cellSize = transform.localScale.divide(new Vector3(SizeX, 1, SizeY));
        return - (transform.localScale / 2f)
            + cellSize / 2f
            + new Vector3((transform.localScale.x / (float)SizeX) * (float)i, 1, (transform.localScale.z / (float)SizeY) * (float)j);
    }
}

public enum MineCollisionType
{
    Proximity, Mine, None
}
