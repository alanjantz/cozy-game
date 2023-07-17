using UnityEngine;
using UnityEngine.Tilemaps;

public class MainAssets : MonoBehaviour
{
    private static MainAssets _instance;

    public Tile[] grassLeaves;
    public Tile grassBlock;
    public Tile wallBlock;
    public Tile wallBlockTop;
    public Transform[] trees;
    public Transform landspace;
    public Tile[] flowers;
    public Transform bench;
    public Transform[] clouds;
    public Transform[] bushes;

    public static MainAssets Instance => _instance;

    public static TileBase GetGrassLeaf(GrassType grassType)
    {
        if (System.Enum.IsDefined(typeof(GrassType), grassType))
            return Instance.grassLeaves[(int)grassType];

        return null;
    }
    public static TileBase GetGrassBlock() => Instance.grassBlock;

    public static TileBase GetWallBlock() => Instance.wallBlock;
    public static TileBase GetWallBlockTop() => Instance.wallBlockTop;

    public static Transform GetTree(TreeType treeType)
    {
        if (System.Enum.IsDefined(typeof(TreeType), treeType))
            return Instance.trees[(int)treeType];

        return null;
    }

    public static Transform GetLandspace() => Instance.landspace;

    public static TileBase GetFlower(FlowerType type)
    {
        if (System.Enum.IsDefined(typeof(FlowerType), type))
            return Instance.flowers[(int)type];

        return null;
    }

    public static Transform GetCloud(CloudType type)
    {
        if (System.Enum.IsDefined(typeof(CloudType), type))
            return Instance.clouds[(int)type];

        return null;
    }

    public static Transform GetBench() => Instance.bench;

    private void Awake()
    {
        _instance = this;
    }
}
