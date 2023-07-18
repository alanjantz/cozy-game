using UnityEngine;

public class Bench : Gadget
{
    public Bench(int position) : base(position)
    {
        Transform = Object.Instantiate(MainAssets.GetBench(), Position, Quaternion.identity);
    }
}
