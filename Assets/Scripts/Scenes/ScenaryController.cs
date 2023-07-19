using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class ScenaryController : MonoBehaviour
{
    private const int SHOWING_CELLS = 30;

    private readonly List<Ground> grounds = new();
    private readonly List<Wall> walls = new();
    private readonly List<Flower> flowers = new();
    private readonly Dictionary<int, Tree> trees = new();
    private readonly Dictionary<int, Landscape> landscapes = new();
    private readonly Dictionary<int, Cloud> clouds = new();
    private readonly Dictionary<int, Gadget> gadgets = new();
    private readonly Dictionary<int, string> storylines = new();

    private Label TextLabel;

    public Tilemap groundMap;
    public Tilemap grassMap;
    public Tilemap flowerMap;
    public Tilemap wallMap;
    public Transform treesContainer;
    public Transform scenaryContainer;
    public Transform gadgetContainer;
    public UIDocument textContainer;

    private void Start()
    {
        groundMap.GetComponent<TilemapCollider2D>().usedByComposite = true;
        TextLabel = textContainer.rootVisualElement.Q<Label>("cozy-text");
        InitializeGround();
        InitializeWall();
        InitializeFlowers();
        InitializeTrees();
        InitializeLandscape();
        InitializeClouds();

        storylines[10] = "Olá";
        storylines[20] = "Nos últimos anos, você me mostrou que presentar pode ter outros significados";
        storylines[30] = "Não é o simples ato de dar um presente";
        storylines[40] = "Não é apenas entregar alguma coisa material";
        storylines[50] = "Por trás desta ação, existem sentimentos, razões e expectativas";
        storylines[60] = "Além disso, cada presente tem uma memória";
        storylines[70] = "E no fim, acho que é isso que importa...";
        storylines[80] = "Comecei a me importar mais com presente quando me dei conta disso";
        storylines[90] = "Por isso, quero que este presente faça parte de suas memórias especiais";
        storylines[100] = "E que você lembre dele quando for preciso";
        storylines[110] = "Pois foi pensando em alguém especial para mim";
        storylines[120] = "Que fiz este presente especial para você.";
        storylines[130] = "Feliz aniversário";
        storylines[140] = "Amo você de montão";
        storylines[150] = "(*^ - ^*)";
    }

    private void Update()
    {
        var cameraPosition = (int)Camera.main.transform.position.x;
        var upcomingTilePosition = cameraPosition + SHOWING_CELLS;
        if (!InputUtils.IsIdle())
        {
            var shouldInsertNewTile = false;
            int positionAux = 1;

            if (InputUtils.IsMovingRight())
                positionAux = -1;
            else if (InputUtils.IsMovingLeft())
                upcomingTilePosition = cameraPosition - SHOWING_CELLS;

            shouldInsertNewTile = !grounds.Any(tile => tile.Position.x == upcomingTilePosition)
                               && !walls.Any(tile => tile.TopPosition.x == upcomingTilePosition)
                               && !flowers.Any(tile => tile.Position.x == upcomingTilePosition);

            if (shouldInsertNewTile)
            {
                SetGroundTile(CreateNewGroundTile(upcomingTilePosition, positionAux));
                SetWallTile(CreateNewWallTile(upcomingTilePosition));
                SetFlowerTile(CreateNewFlowerTile(upcomingTilePosition));
            }

            ControlTrees(upcomingTilePosition);
            ControlLandscape(upcomingTilePosition);
        }

        ControlClouds(upcomingTilePosition);

        ControlText(cameraPosition);
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

    #region Flower
    private void InitializeFlowers()
    {
        for (int cell = SHOWING_CELLS * -1; cell < SHOWING_CELLS; cell++)
            flowers.Add(CreateNewFlowerTile(cell));

        foreach (var flower in flowers)
            SetFlowerTile(flower);
    }

    private Flower CreateNewFlowerTile(int upcomingTilePosition)
    {
        return new Flower(upcomingTilePosition, (FlowerType)Random.Range(0, MainAssets.Instance.flowers.Length));
    }

    private void SetFlowerTile(Flower flower)
    {
        if (Random.value <= 0.75f)
            flowerMap.SetTile(flower.Position, MainAssets.GetFlower(flower.Type));
    }
    #endregion Flower

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
                    currentTree.Transform.parent = treesContainer.transform;
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
        tree.Transform.parent = treesContainer.transform;
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
                    landscapesTree.Transform.parent = treesContainer.transform;
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
        landscape.Transform.parent = scenaryContainer.transform;
        return landscape;
    }

    #endregion Landscape

    #region Cloud
    private void InitializeClouds()
    {
        int startingClouds = 8;

        for (int i = 0, position = -SHOWING_CELLS; i < startingClouds; i++)
        {
            clouds[position] = CreateCloud(position);
            position += Random.Range(8, 10);
        }
    }

    private void ControlClouds(int upcomingTilePosition)
    {
        if (!clouds.Keys.Any(cloudPosition => upcomingTilePosition - 8 <= cloudPosition && upcomingTilePosition + 8 >= cloudPosition))
        {
            clouds[upcomingTilePosition] = CreateCloud(upcomingTilePosition);
        }
        else
        {
            foreach (var cloudPosition in clouds.Keys.Where(cloudPosition => upcomingTilePosition - 8 <= cloudPosition && upcomingTilePosition + 8 >= cloudPosition))
            {
                var currentCloud = clouds[cloudPosition];
                if (!currentCloud.Active)
                {
                    currentCloud.Instantiate();
                    currentCloud.Transform.parent = scenaryContainer.transform;
                }
            }
        }

        foreach (var cloudPosition in clouds.Keys.Where(cloudPosition => cloudPosition < upcomingTilePosition - SHOWING_CELLS * 2 || cloudPosition > upcomingTilePosition + SHOWING_CELLS * 2))
        {
            clouds[cloudPosition].Destroy();
        }
    }

    private Cloud CreateCloud(int position)
    {
        var cloud = new Cloud((CloudType)Random.Range(0, MainAssets.Instance.clouds.Length), position);
        cloud.Transform.parent = scenaryContainer.transform;
        return cloud;
    }
    #endregion Cloud

    #region Text

    private void ControlText(int cameraPosition)
    {
        bool filterPositions(int position)
            => position >= cameraPosition - 3 && position <= cameraPosition + 3;

        if (storylines.Keys.Any(position => filterPositions(position)))
        {
            var position = storylines.Keys.FirstOrDefault(position => (filterPositions(position)));
            if (storylines.ContainsKey(position))
            {
                TextLabel.visible = true;
                TextLabel.text = storylines[position];
            }
        }
        else
        {
            TextLabel.visible = false;
            TextLabel.text = null;
        }
    }

    #endregion Text
}
