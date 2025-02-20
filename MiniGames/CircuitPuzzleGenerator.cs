using UnityEngine;
using System.Collections.Generic;

public class CircuitPuzzleGenerator : MonoBehaviour
{
    public static CircuitPuzzleGenerator instance;
    public int gridSize = 5;
    public float tileSize = 1f;
    public GameObject[] tilePrefabs;
    public GameObject generator;
    MazeCell[,] grid;
    public Wires[,] wireGrid;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        GenerateMaze();
        InstantiateTiles();

        // for(int x = 0; x < gridSize; x++) //turn everything off
        // {
        //     for(int y = 0; y < gridSize; y++)
        //     {
        //         wireGrid[x, y].SetActivity(false);
        //     }
        // }

        for(int x = 0; x < gridSize; x++)
        {
            for(int y = 0; y < gridSize; y++)
            {
                if (wireGrid[x,y].isGenerator) {
                    wireGrid[x,y].CheckNeighborActivity();
                }
            }
        }
    }

    void GenerateMaze()
    {
        grid = new MazeCell[gridSize, gridSize];
        for (int x = 0; x < gridSize; x++)
            for (int y = 0; y < gridSize; y++)
                grid[x, y] = new MazeCell();
        Stack<Vector2Int> stack = new Stack<Vector2Int>();
        Vector2Int current = new Vector2Int(0, 0);
        grid[0, 0].visited = true;
        do
        {
            List<Vector2Int> neighbors = new List<Vector2Int>();
            if (current.y < gridSize - 1 && !grid[current.x, current.y + 1].visited) neighbors.Add(new Vector2Int(current.x, current.y + 1));
            if (current.x < gridSize - 1 && !grid[current.x + 1, current.y].visited) neighbors.Add(new Vector2Int(current.x + 1, current.y));
            if (current.y > 0 && !grid[current.x, current.y - 1].visited) neighbors.Add(new Vector2Int(current.x, current.y - 1));
            if (current.x > 0 && !grid[current.x - 1, current.y].visited) neighbors.Add(new Vector2Int(current.x - 1, current.y));
            if (neighbors.Count > 0)
            {
                Vector2Int chosen = neighbors[Random.Range(0, neighbors.Count)];
                RemoveWall(current, chosen);
                stack.Push(current);
                current = chosen;
                grid[current.x, current.y].visited = true;
            }
            else if (stack.Count > 0)
            {
                current = stack.Pop();
            }
        } while (stack.Count > 0);
    }

    void RemoveWall(Vector2Int current, Vector2Int chosen)
    {
        int dx = chosen.x - current.x;
        int dy = chosen.y - current.y;
        if (dx == 1)
        {
            grid[current.x, current.y].walls[1] = false;
            grid[chosen.x, chosen.y].walls[3] = false;
        }
        else if (dx == -1)
        {
            grid[current.x, current.y].walls[3] = false;
            grid[chosen.x, chosen.y].walls[1] = false;
        }
        else if (dy == 1)
        {
            grid[current.x, current.y].walls[0] = false;
            grid[chosen.x, chosen.y].walls[2] = false;
        }
        else if (dy == -1)
        {
            grid[current.x, current.y].walls[2] = false;
            grid[chosen.x, chosen.y].walls[0] = false;
        }
    }

    bool IsStraight(int mask)
    {
        List<int> setBits = new List<int>();
        for (int i = 0; i < 4; i++)
        {
            if ((mask & (1 << i)) != 0) setBits.Add(i);
        }
        if (setBits.Count != 2) return false;
        int diff = Mathf.Abs(setBits[0] - setBits[1]);
        return diff == 2;
    }

    void InstantiateTiles()
    {
        wireGrid = new Wires[gridSize, gridSize];
        float maxWidth = tileSize * gridSize;
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                Vector3 pos = transform.position + new Vector3(x*tileSize - (maxWidth/2), y*tileSize - (maxWidth/2), 0);
                int mask = 0;
                if (!grid[x, y].walls[0]) mask |= 1;
                if (!grid[x, y].walls[1]) mask |= 2;
                if (!grid[x, y].walls[2]) mask |= 4;
                if (!grid[x, y].walls[3]) mask |= 8;
                int connectionCount = CountConnections(mask);
                GameObject prefabToUse;

                if (IsDeadEnd(grid[x, y])) prefabToUse = generator;
                else prefabToUse = tilePrefabs[connectionCount - 1];
                if (connectionCount == 2 && IsStraight(mask)) prefabToUse = tilePrefabs[0];

                GameObject tile = Instantiate(prefabToUse, pos, Quaternion.identity, transform);
                tile.transform.localScale = Vector3.one * tileSize;

                Wires wires = tile.GetComponent<Wires>();
                wires.gridX = x;
                wires.gridY = y;

                if (IsDeadEnd(grid[x, y])) {
                    wires.isGenerator = true;
                    wires.SetConnections(mask, WireTypes.gen);
                }
                else
                {
                    wires.SetConnections(mask, (WireTypes)connectionCount);
                    int rotations = Random.Range(0,4);
                    for (int i = 0; i < rotations; i++) wires.RotateTile();
                }
                wireGrid[x, y] = wires;
            }
        }
    }

    public Wires GetWireAt(int x, int y)
    {
        if (x < 0 || y < 0 || x >= gridSize || y >= gridSize) return null;
        return wireGrid[x, y];
    }

    int CountConnections(int mask)
    {
        int count = 0;
        for (int i = 0; i < 4; i++)
        {
            if ((mask & (1<<i)) != 0) count++;
        }
        return count;
    }

    bool IsDeadEnd(MazeCell cell)
    {
        int openCount = 0;
        for (int i = 0; i < 4; i++)
            if (!cell.walls[i]) openCount++;
        return openCount == 1;
    }

    public void CheckWinCondition()
    {
        // for(int x = 0; x < gridSize; x++)
        // {
        //     for(int y = 0; y < gridSize; y++)
        //     {
        //         Wires w = wireGrid[x,y];
        //         if (!w.isGenerator && CountConnections(w.connections) > 0 && !w.isActive)
        //         {
        //             return; 
        //         }
        //     }
        // }
        // Debug.Log("Puzzle Solved!");
    }
}
public class MazeCell
{
    public bool visited = false;
    public bool[] walls = { true, true, true, true };
}
