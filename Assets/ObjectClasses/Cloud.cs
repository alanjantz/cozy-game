using UnityEngine;

public class Cloud
{
    public CloudType Type { get; private set; }
    public Vector3 Position { get; private set; }
    public Transform Transform { get; private set; }
    public bool Active { get; private set; }

    public Cloud(CloudType cloudType, int position)
    {
        Type = cloudType;
        Position = new Vector3(position, Random.Range(6f, 16f));
        Instantiate();
    }

    public void Instantiate()
    {
        if (!Active)
        {
            Transform = Object.Instantiate(MainAssets.GetCloud(Type), Position, Quaternion.identity);
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
