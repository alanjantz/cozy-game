using System.Collections.Generic;
using UnityEngine;

public class Wall
{
    public const int STARTING_WALL_CELL = 0;
    public const int WALL_HEIGHT = 1;

    public List<Vector3Int> BlockPositions { get; } = new();
    public Vector3Int TopPosition { get; private set; }

    public Wall(int cell)
    {
        TopPosition = new Vector3Int(cell, STARTING_WALL_CELL + WALL_HEIGHT);

        for (int cellY = STARTING_WALL_CELL; cellY < STARTING_WALL_CELL + WALL_HEIGHT; cellY++)
            BlockPositions.Add(new(cell, cellY));
    }
}
