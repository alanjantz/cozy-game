using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CozyController : MonoBehaviour
{
    private const int SHOWING_CELLS = 30;

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
        if (!InputUtils.IsIdle())
        {
            var cameraPosition = (int)Camera.main.transform.position.x;
            var upcomingTilePosition = cameraPosition + SHOWING_CELLS;
            var shouldInsertNewTile = false;
            int positionAux = 1;

            if (InputUtils.IsMovingRight())
                positionAux = -1;
            else if (InputUtils.IsMovingLeft())
                upcomingTilePosition = cameraPosition - SHOWING_CELLS;

            shouldInsertNewTile = !grounds.Any(tile => tile.Position.x == upcomingTilePosition)
                               && !walls.Any(tile => tile.TopPosition.x == upcomingTilePosition);

            if (shouldInsertNewTile)
            {
                SetGroundTile(CreateNewGroundTile(upcomingTilePosition, positionAux));
                SetWallTile(CreateNewWallTile(upcomingTilePosition));
            }
        }
    }

    private void InitializeGround()
    {
        var lastGrassType = GrassType.First;
        for (int cell = SHOWING_CELLS * -1; cell < SHOWING_CELLS; cell++)
        {
            var ground = new Ground(cell, lastGrassType);
            grounds.Add(ground);
            lastGrassType = ground.GetNextGrassType();
        }

        foreach (var ground in grounds)
            SetGroundTile(ground);
    }

    private void InitializeWall()
    {
        for (int cell = SHOWING_CELLS * -1; cell < SHOWING_CELLS; cell++)
            walls.Add(new(cell));

        foreach (var wall in walls)
            SetWallTile(wall);
    }

    private Ground CreateNewGroundTile(int upcomingTilePosition, int positionAux)
    {
        var lastGround = grounds.FirstOrDefault(tile => tile.Position.x == upcomingTilePosition + positionAux);
        var newGround = new Ground(upcomingTilePosition, lastGround.GetPreviousGrassType());
        grounds.Add(newGround);
        return newGround;
    }

    private void SetGroundTile(Ground ground)
    {
        grassMap.SetTile(ground.Position, MainAssets.GetGrassLeaf(ground.GrassType));
        foreach (var underground in ground.Underground)
            groundMap.SetTile(underground, MainAssets.GetGrassBlock());
    }

    private Wall CreateNewWallTile(int upcomingTilePosition)
    {
        var newWall = new Wall(upcomingTilePosition);
        walls.Add(newWall);
        return newWall;
    }

    private void SetWallTile(Wall wall)
    {
        wallMap.SetTile(wall.TopPosition, MainAssets.GetWallBlockTop());
        foreach (var blockPosition in wall.BlockPositions)
            wallMap.SetTile(blockPosition, MainAssets.GetWallBlock());
    }
}
