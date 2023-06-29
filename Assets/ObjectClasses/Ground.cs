using System.Collections.Generic;
using UnityEngine;

public class Ground
{
    public const int GROUND_HEIGHT = 8;
    public const int STARTING_GRASS_CELL = 0;
    public const int STARTING_GROUND_CELL = -1;

    public Vector3Int Position { get; private set; }
    public GrassType GrassType { get; private set; }
    public List<Vector3Int> Underground { get; } = new();

    public Ground(int cell, GrassType grassType)
    {
        Position = new(cell, STARTING_GRASS_CELL);
        GrassType = grassType;

        CreateUnderground();
    }

    private void CreateUnderground()
    {
        for (int i = 0, cellPosition = STARTING_GROUND_CELL; i < GROUND_HEIGHT; i++, cellPosition--)
            Underground.Add(new(Position.x, cellPosition));
    }

    public GrassType GetNextGrassType()
    {
        return GrassType switch
        {
            GrassType.First => GrassType.Second,
            GrassType.Second => GrassType.Third,
            _ => GrassType.First,
        };
    }

    public GrassType GetPreviousGrassType()
    {
        return GrassType switch
        {
            GrassType.Third => GrassType.Second,
            GrassType.Second => GrassType.First,
            _ => GrassType.Third,
        };
    }
}
