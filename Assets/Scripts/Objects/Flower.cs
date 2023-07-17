using UnityEngine;

public class Flower
{
    public const int STARTING_FLOWER_CELL = 0;

    public FlowerType Type { get; set; }
    public Vector3Int Position { get; private set; }
    public Transform Transform { get; private set; }

    public Flower(int cell, FlowerType type)
    {
        Position = new(cell, STARTING_FLOWER_CELL);
        Type = type;
    }
}
