using UnityEngine;

public class CloudMovement : MonoBehaviour
{
    public float moveSpeed = 0.5f;

    void Update()
    {
        transform.position += Time.deltaTime * new Vector3(moveSpeed, 0, 0);
    }
}
