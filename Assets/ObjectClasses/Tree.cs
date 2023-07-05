using UnityEngine;

public class Tree
{
    public const int STARTING_TREE_HEIGHT = 6;

    public TreeType TreeType { get; private set; }
    public Vector3 Position { get; private set; }
    public Transform Transform { get; private set; }

    public Tree(TreeType treeType, int position)
    {
        TreeType = treeType;
        Position = new Vector3(position, STARTING_TREE_HEIGHT);
        Transform = Object.Instantiate(MainAssets.GetTree(TreeType), Position, Quaternion.identity);
    }
}
