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

    private void Awake()
    {
        _instance = this;
    }
}
