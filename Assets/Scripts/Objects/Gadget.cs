using UnityEngine;

public abstract class Gadget
{
    public Vector3 Position { get; private set; }
    public Transform Transform { get; protected set; }

    protected abstract float y { get; }

    protected Gadget(int position)
    {
        Position = new Vector3(position, y);
    }
}
