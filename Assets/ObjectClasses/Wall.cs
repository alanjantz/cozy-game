using System.Collections.Generic;
using UnityEngine;

public class Wall
{
    public List<Vector3Int> BlockPositions { get; } = new();
    public Vector3Int TopPosition { get; private set; }

    public Wall(int startingCell, int currentCell, int blockHeight)
    {
        TopPosition = new Vector3Int(currentCell, startingCell + blockHeight);

        for (int cellY = startingCell; cellY < startingCell + blockHeight; cellY++)
            BlockPositions.Add(new(currentCell, cellY));
    }
}
