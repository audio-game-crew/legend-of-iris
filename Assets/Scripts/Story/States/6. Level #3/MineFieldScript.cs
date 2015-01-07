using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MineFieldScript : MonoBehaviour {
    public int SizeX = 10;
    public int SizeY = 10;
    public int MineCount = 7;
    public float GridCellSize = 2f;
    public int MaxPlacementTries = 1000;
    public Point2 StartPos = new Point2();
    public Point2 EndPos = new Point2(10, 10);
    public GameObject Mine;
    public GameObject MineProximitySensor;
    public AudioClip MineSound;

    private bool[,] grid;
    private List<GameObject> mines = new List<GameObject>();
    Dictionary<GameObject, GameObject> mineProximitySensors = new Dictionary<GameObject, GameObject>();
    private Dictionary<GameObject, AudioPlayer> minePlayers = new Dictionary<GameObject, AudioPlayer>();
    private Dictionary<GameObject, Point2> minePositions = new Dictionary<GameObject, Point2>();
    public GameObject[] Mines { get { return mines.ToArray(); } }
    private int placementTries = 0;

	// Use this for initialization
	void Start () 
    {
        if (OutsideGrid(StartPos))
            Debug.LogWarning("Start Position is outside the grid");
        if (OutsideGrid(EndPos))
            Debug.LogWarning("End Position is outside the grid");
        grid = new bool[SizeX, SizeY];
        placementTries = 0;
        PlaceMines();
        AddMines();
	}

    private void ResetGrid()
    {
        for (int i = 0; i < SizeX; i++)
            for (int j = 0; j < SizeY; j++)
            {
                grid[i, j] = false;
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
        if (!PathExists() && placementTries < MaxPlacementTries)
            PlaceMines();
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
        foreach (var mine in mines)
            GameObject.Destroy(mine);
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
        var minePlayer = AudioManager.PlayAudio(new AudioObject(mine, MineSound, 1, delay, true));
        if (MineProximitySensor != null)
        {
            var mineProximitySensor = (GameObject)GameObject.Instantiate(MineProximitySensor, minePosition, new Quaternion());
            mineProximitySensor.transform.parent = this.transform;
            mineProximitySensors.Add(mine, mineProximitySensor);
        }
        mines.Add(mine);
        minePlayers.Add(mine, minePlayer);
        minePositions.Add(mine, new Point2(i, j));
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
        if (!minePlayers.ContainsKey(mine))
        {
            Debug.LogError("Mine not found!");
            return;
        }
        minePlayers[mine].SetPitch(trigger ? 2 : 1);
    }

    public void RemoveMine(GameObject mineToRemove)
    {
        foreach (var mine in mines)
        {
            if (mine == mineToRemove)
            {
                if (mineProximitySensors.ContainsKey(mineToRemove))
                    mineProximitySensors[mineToRemove].SetActive(false);
                minePlayers[mine].MarkRemovable();
                mine.SetActive(false);
                var minePosition = minePositions[mine];
                grid[minePosition.x, minePosition.y] = false;
            }
        }
    }

	// Update is called once per frame
	void Update () {
	
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
