using UnityEngine;
using UnityEngine.Tilemaps;

public class MainAssets : MonoBehaviour
{
    private static MainAssets _instance;

    [SerializeField]
    public Tile[] GrassLeaves;
    [SerializeField]
    public Tile GrassBlock;

    public static MainAssets Instance => _instance;

    private void Awake()
    {
        _instance = this;
    }
}
