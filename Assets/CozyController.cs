using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CozyController : MonoBehaviour
{
    private List<Ground> grounds;

    public Tilemap groundMap;
    public Tilemap grassMap;
    public Tilemap wallMap;
    public Tilemap treesMap;
    public Tilemap scenaryMap;

    void Start()
    {
        groundMap.GetComponent<TilemapCollider2D>().usedByComposite = true;
        InitializeGround();
    }

    void Update()
    {

    }

    private void InitializeGround()
    {
        const int SHOWING_CELLS = 10;
        const int STARTING_GRASS_CELL = 0;
        grounds = new();

        for (int cell = SHOWING_CELLS * -1, lastTile = 0; cell < SHOWING_CELLS; cell++)
        {
            grounds.Add(new(new(cell, STARTING_GRASS_CELL), MainAssets.Instance.grassLeaves[lastTile]));

            lastTile++;
            if (lastTile == MainAssets.Instance.grassLeaves.Length)
                lastTile = 0;
        }

        foreach (var ground in grounds)
        {
            grassMap.SetTile(ground.Position, ground.Tile);
            foreach (var underground in ground.Underground)
                groundMap.SetTile(underground.Key, underground.Value);
        }
    }

    private class Ground
    {
        private const int GROUND_HEIGHT = 4;
        private const int STARTING_GROUND_CELL = -1;

        public Vector3Int Position { get; private set; }
        public TileBase Tile { get; private set; }
        public Dictionary<Vector3Int, TileBase> Underground { get; } = new();

        public Ground(Vector3Int position, TileBase tile)
        {
            Position = position;
            Tile = tile;

            CreateUnderground();
        }

        private void CreateUnderground()
        {
            for (int i = 0, cellPosition = STARTING_GROUND_CELL; i < GROUND_HEIGHT; i++, cellPosition--)
                Underground[new(Position.x, cellPosition)] = MainAssets.Instance.grassBlock;
        }
    }
}
