using UnityEngine;

public class CloudMovement : MonoBehaviour
{
    public float moveSpeed = 0.5f;

    private float _initialPosition;
    private const int MAX_DISTANCE = 3;

    private void Start()
    {
        _initialPosition = transform.position.x;
    }

    void Update()
    {
        if (transform.position.x > _initialPosition + MAX_DISTANCE || transform.position.x < _initialPosition - MAX_DISTANCE)
            moveSpeed *= -1;

        transform.position += Time.deltaTime / 2 * new Vector3(moveSpeed, 0, 0);
    }
}
