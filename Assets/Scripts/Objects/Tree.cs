using UnityEngine;

public class Tree
{
    public TreeType Type { get; private set; }
    public Vector3 Position { get; private set; }
    public Transform Transform { get; private set; }
    public bool Active { get; private set; }

    public Tree(TreeType treeType, int position)
    {
        Type = treeType;
        Position = new Vector3(position, GetHeighByType());
        Instantiate();
    }

    public void Instantiate()
    {
        if (!Active)
        {
            Transform = Object.Instantiate(MainAssets.GetTree(Type), Position, Quaternion.identity);
            Active = true;
        }
    }

    public void Destroy()
    {
        if (Active)
        {
            Object.Destroy(Transform.gameObject);
            Transform = null;
            Active = false;
        }
    }

    private float GetHeighByType()
    {
        return Type switch
        {
            TreeType.First => 6.9f,
            TreeType.Second => 8.45f,
            TreeType.Third => 6.5f,
            _ => 2f,
        };
    }
}
