
using UnityEngine;

public class SignPost : Gadget
{
    protected override float y => 1.7f;

    public SignPost(int position) : base(position)
    {
        Transform = Object.Instantiate(MainAssets.GetSignPost(), Position, Quaternion.identity);
    }
}