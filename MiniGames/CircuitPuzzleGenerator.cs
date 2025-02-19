using UnityEngine;
using System.Collections.Generic;

public class CircuitPuzzleGenerator : MonoBehaviour
{
    public int gridSize = 5;
    public float tileSize = 1f;
    public GameObject[] tilePrefabs;
    public GameObject generator;
    MazeCell[,] grid;

    void Start()
    {
        GenerateMaze();
        InstantiateTiles();
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
            if (current.y < gridSize - 1 && !grid[current.x, current.y + 1].visited)
                neighbors.Add(new Vector2Int(current.x, current.y + 1));
            if (current.x < gridSize - 1 && !grid[current.x + 1, current.y].visited)
                neighbors.Add(new Vector2Int(current.x + 1, current.y));
            if (current.y > 0 && !grid[current.x, current.y - 1].visited)
                neighbors.Add(new Vector2Int(current.x, current.y - 1));
            if (current.x > 0 && !grid[current.x - 1, current.y].visited)
                neighbors.Add(new Vector2Int(current.x - 1, current.y));

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

    void InstantiateTiles()
    {
        float maxWidth = tileSize * gridSize;
        for (int x = 0; x < gridSize; x++)
            for (int y = 0; y < gridSize; y++)
            {
                Vector3 pos = transform.position + new Vector3(x * tileSize - (maxWidth/2), y * tileSize - (maxWidth/2), 0);

                int mask = 0;
                if (!grid[x, y].walls[0]) mask |= 1;
                if (!grid[x, y].walls[1]) mask |= 2;
                if (!grid[x, y].walls[2]) mask |= 4;
                if (!grid[x, y].walls[3]) mask |= 8;

                int connectionCount = CountConnections(mask);

                GameObject prefabToUse;
                
                if (IsDeadEnd(grid[x, y])){
                    prefabToUse = generator;
                } else{
                    prefabToUse = tilePrefabs[connectionCount];
                }
                GameObject tile = Instantiate(prefabToUse, pos, Quaternion.identity, transform);;
                tile.transform.localScale = Vector3.one * tileSize;
                Wires wires = tile.GetComponent<Wires>();

                wires.SetConnections(mask);

                if (IsDeadEnd(grid[x, y]))
                    wires.isGenerator = true;

                int rotations = Random.Range(0, 4);
                for (int i = 0; i < rotations; i++)
                    wires.RotateTile();
            }
    }

    private int CountConnections(int mask){
        int count = 0;
        for(int i = 0; i < 4; i++){ //mask is 4 bits
            if((mask & (1<<i)) != 0){ //move the 1 in 0001 over i amount, so 0100 for 3 aka 8 (same as mask)
                count++; //for every bit with nothing there a count is added
            }
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
}


public class MazeCell
{
    public bool visited = false;
    public bool[] walls = { true, true, true, true };
}
