using System.Collections.Generic;
using UnityEngine;

public class Ground
{
    public const int GROUND_HEIGHT = 4;
    public const int STARTING_GROUND_CELL = -1;

    public Vector3Int Position { get; private set; }
    public GrassType GrassType { get; private set; }
    public List<Vector3Int> Underground { get; } = new();

    public Ground(Vector3Int position, GrassType grassType)
    {
        Position = position;
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
