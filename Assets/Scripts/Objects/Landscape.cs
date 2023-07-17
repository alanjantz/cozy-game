
using UnityEngine;

public class Landscape
{
    public const int WIDTH = 9;
    public const int HEIGHT = 2;
    private const int STARTING_CELL = 2;

    public Vector3 Position { get; private set; }
    public Transform Transform { get; private set; }
    public bool Active { get; private set; }

    public Landscape(int position)
    {
        Position = new(position, STARTING_CELL);
        Instantiate();
    }

    public void Instantiate()
    {
        if (!Active)
        {
            Transform = Object.Instantiate(MainAssets.GetLandspace(), Position, Quaternion.identity);
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
}