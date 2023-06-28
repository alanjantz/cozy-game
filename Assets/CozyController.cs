using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CozyController : MonoBehaviour
{
    private List<Ground> grounds;

    [SerializeField]
    public Tilemap GroundMap;
    [SerializeField]
    public Tilemap GrassMap;

    void Start()
    {
        GroundMap.GetComponent<TilemapCollider2D>().usedByComposite = true;
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
            grounds.Add(new(new(cell, STARTING_GRASS_CELL), MainAssets.Instance.GrassLeaves[lastTile]));

            lastTile++;
            if (lastTile == MainAssets.Instance.GrassLeaves.Length)
                lastTile = 0;
        }

        foreach (var ground in grounds)
        {
            GrassMap.SetTile(ground.Position, ground.Tile);
            foreach (var underground in ground.Underground)
                GroundMap.SetTile(underground.Key, underground.Value);
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
                Underground[new(Position.x, cellPosition)] = MainAssets.Instance.GrassBlock;
        }
    }
}
