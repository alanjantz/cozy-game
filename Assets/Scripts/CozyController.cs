using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CozyController : MonoBehaviour
{
    private const int SHOWING_CELLS = 30;

    private readonly List<Ground> grounds = new();
    private readonly List<Wall> walls = new();
    private readonly Dictionary<int, Tree> trees = new();
    private readonly Dictionary<int, Landscape> landscapes = new();

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
        InitializeTrees();
        InitializeLandscape();
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

            ControlTrees(upcomingTilePosition);
            ControlLandscape(upcomingTilePosition);
        }
    }

    #region Ground
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
    #endregion Ground

    #region Wall
    private void InitializeWall()
    {
        for (int cell = SHOWING_CELLS * -1; cell < SHOWING_CELLS; cell++)
            walls.Add(new(cell));

        foreach (var wall in walls)
            SetWallTile(wall);
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
    #endregion Wall

    #region Tree
    private void InitializeTrees()
    {
        int startingTrees = 8;

        for (int i = 0, position = -SHOWING_CELLS; i < startingTrees; i++)
        {
            trees[position] = CreateTree(position);
            position += Random.Range(8, 10);
        }
    }

    private void ControlTrees(int upcomingTilePosition)
    {
        if (!trees.Keys.Any(treePosition => upcomingTilePosition - 8 <= treePosition && upcomingTilePosition + 8 >= treePosition))
        {
            var position = upcomingTilePosition + Random.Range(0, 2);
            trees[position] = CreateTree(position);
        }
        else
        {
            foreach (var treePosition in trees.Keys.Where(treePosition => upcomingTilePosition - 8 <= treePosition && upcomingTilePosition + 8 >= treePosition))
            {
                var currentTree = trees[treePosition];
                if (!currentTree.Active)
                {
                    currentTree.Instantiate();
                    currentTree.Transform.parent = treesMap.transform;
                }
            }
        }

        foreach (var treePosition in trees.Keys.Where(treePosition => treePosition < upcomingTilePosition - SHOWING_CELLS * 2 || treePosition > upcomingTilePosition + SHOWING_CELLS * 2))
        {
            trees[treePosition].Destroy();
        }
    }

    private Tree CreateTree(int position)
    {
        var tree = new Tree((TreeType)Random.Range(0, MainAssets.Instance.trees.Length), position);
        tree.Transform.parent = treesMap.transform;
        return tree;
    }
    #endregion Tree

    #region Lanscape

    private void InitializeLandscape()
    {
        int startingLandscape = 8;

        for (int i = 0, position = -SHOWING_CELLS; i < startingLandscape; i++)
        {
            landscapes[position] = CreateLandscape(position);
            position += Landscape.WIDTH;
        }
    }

    private void ControlLandscape(int upcomingTilePosition)
    {
        if (!landscapes.Keys.Any(landscapePosition => upcomingTilePosition + Landscape.WIDTH > landscapePosition && upcomingTilePosition - Landscape.WIDTH < landscapePosition))
        {
            var position = upcomingTilePosition;
            landscapes[position] = CreateLandscape(position);
        }
        else
        {
            foreach (var landscapePosition in landscapes.Keys.Where(landscapePosition => upcomingTilePosition + Landscape.WIDTH > landscapePosition && upcomingTilePosition - Landscape.WIDTH < landscapePosition))
            {
                var landscapesTree = landscapes[landscapePosition];
                if (!landscapesTree.Active)
                {
                    landscapesTree.Instantiate();
                    landscapesTree.Transform.parent = treesMap.transform;
                }
            }
        }

        foreach (var landscapePosition in landscapes.Keys.Where(landscapePosition => landscapePosition < upcomingTilePosition - SHOWING_CELLS * 2 || landscapePosition > upcomingTilePosition + SHOWING_CELLS * 2))
        {
            landscapes[landscapePosition].Destroy();
        }
    }

    private Landscape CreateLandscape(int position)
    {
        var landscape = new Landscape(position);
        landscape.Transform.parent = scenaryMap.transform;
        return landscape;
    }

    #endregion Landscape
}