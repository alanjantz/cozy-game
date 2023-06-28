using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MainAssets : MonoBehaviour
{
    private static MainAssets _instance;

    public Tile[] grassLeaves;
    public Tile grassBlock;
    public Tile wallBlock;
    public Tile wallBlockTop;

    public static MainAssets Instance => _instance;

    public static TileBase GetGrassLeaf(GrassType grassType)
    {
        if (Enum.IsDefined(typeof(GrassType), grassType))
            return Instance.grassLeaves[(int)grassType];

        return null;
    }
    public static TileBase GetGrassBlock() => Instance.grassBlock;

    public static TileBase GetWallBlock() => Instance.wallBlock;
    public static TileBase GetWallBlockTop() => Instance.wallBlockTop;

    private void Awake()
    {
        _instance = this;
    }
}
