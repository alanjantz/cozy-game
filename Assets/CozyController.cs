using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CozyController : MonoBehaviour
{
    private const int SHOWING_CELLS = 10;
    private const int STARTING_GRASS_CELL = 0;

    private readonly List<Ground> grounds = new();
    private readonly List<Wall> walls = new();

    public Tilemap groundMap;
    public Tilemap grassMap;
    public Tilemap wallMap;
    public Tilemap treesMap;
    public Tilemap scenaryMap;

    private void Start()
    {
        groundMap.GetComponent<TilemapCollider2D>().usedByComposite = true;
        InitializeGround();
        InitializeWall();

    }

    private void Update()
    {
        var userDirection = Input.GetAxisRaw("Horizontal");

        if (userDirection > 0 /* right */)
        {
            var cameraPosition = Camera.main.transform.position;
            var upcomingTilePosition = (int)cameraPosition.x + SHOWING_CELLS;
            if (!grounds.Any(tile => tile.Position.x == upcomingTilePosition))
            {
                var lastGround = grounds.FirstOrDefault(tile => tile.Position.x == upcomingTilePosition - 1);
                var newGround = new Ground(new(upcomingTilePosition, STARTING_GRASS_CELL), lastGround.GetNextGrassType());
                grounds.Add(newGround);
                InsertGroundTile(newGround);
            }
        }
        else if (userDirection < 0 /* left */)
        {
            var cameraPosition = Camera.main.transform.position;
            var upcomingTilePosition = (int)cameraPosition.x - SHOWING_CELLS;
            if (!grounds.Any(tile => tile.Position.x == upcomingTilePosition))
            {
                var lastGround = grounds.FirstOrDefault(tile => tile.Position.x == upcomingTilePosition + 1);
                var newGround = new Ground(new(upcomingTilePosition, STARTING_GRASS_CELL), lastGround.GetPreviousGrassType());
                grounds.Add(newGround);
                InsertGroundTile(newGround);
            }
        }
    }

    private void InitializeGround()
    {
        var lastGrassType = GrassType.First;
        for (int cell = SHOWING_CELLS * -1; cell < SHOWING_CELLS; cell++)
        {
            var ground = new Ground(new(cell, STARTING_GRASS_CELL), lastGrassType);
            grounds.Add(ground);
            lastGrassType = ground.GetNextGrassType();
        }

        foreach (var ground in grounds)
            InsertGroundTile(ground);
    }

    private void InitializeWall()
    {
        const int WALL_HEIGHT = 1;
        const int STARTING_WALL_CELL = 0;

        for (int cell = SHOWING_CELLS * -1; cell < SHOWING_CELLS; cell++)
            walls.Add(new(STARTING_WALL_CELL, cell, WALL_HEIGHT));

        foreach (var wall in walls)
        {
            wallMap.SetTile(wall.TopPosition, MainAssets.GetWallBlockTop());
            foreach (var blockPosition in wall.BlockPositions)
                wallMap.SetTile(blockPosition, MainAssets.GetWallBlock());
        }
    }

    private void InsertGroundTile(Ground ground)
    {
        grassMap.SetTile(ground.Position, MainAssets.GetGrassLeaf(ground.GrassType));
        foreach (var underground in ground.Underground)
            groundMap.SetTile(underground, MainAssets.GetGrassBlock());
    }
}
