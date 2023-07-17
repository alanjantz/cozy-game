using UnityEngine;

public class Bench
{
    public Vector3 Position { get; private set; }
    public Transform Transform { get; private set; }

    public Bench(int position)
    {
        Position = new Vector3(position, 1.4f);
        Transform = Object.Instantiate(MainAssets.GetBench(), Position, Quaternion.identity);
    }
}
