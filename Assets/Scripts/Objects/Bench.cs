using UnityEngine;

public class Bench : Gadget
{
    protected override float y => 1.4f;

    public Bench(int position) : base(position)
    {
        Transform = Object.Instantiate(MainAssets.GetBench(), Position, Quaternion.identity);
    }
}
