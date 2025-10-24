using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Range(1, 10)] [SerializeField] private float speed;
    [SerializeField] private float reducedVelocity = 2f;

    private Rigidbody2D rb2d;
    private Camera mainCamera;


    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
    }

    void FixedUpdate()
    {
        float horizontalMovement = 0;
        float verticalMovement = 0;

        if (Input.GetKey(InputManager.Instance.GetKey("MoveLeft"))) horizontalMovement = -1;
        if (Input.GetKey(InputManager.Instance.GetKey("MoveRight"))) horizontalMovement = 1;
        if (Input.GetKey(InputManager.Instance.GetKey("MoveUp"))) verticalMovement = 1;
        if (Input.GetKey(InputManager.Instance.GetKey("MoveDown"))) verticalMovement = -1;

        Vector2 movement = Input.GetKey(InputManager.Instance.GetKey("FocusFireMode")) ?
            new Vector2(horizontalMovement * speed / reducedVelocity, verticalMovement * speed / reducedVelocity) :
            new Vector2(horizontalMovement * speed, verticalMovement * speed);

        Vector3 newPosition = rb2d.position + movement * Time.deltaTime;
        newPosition = ClampToCameraBounds(newPosition);

        rb2d.MovePosition(newPosition);
    }


    private Vector3 ClampToCameraBounds(Vector3 targetPosition)
    {
        Vector3 minBounds = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, mainCamera.nearClipPlane));
        Vector3 maxBounds = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, mainCamera.nearClipPlane));

        targetPosition.x = Mathf.Clamp(targetPosition.x, minBounds.x, maxBounds.x);
        targetPosition.y = Mathf.Clamp(targetPosition.y, minBounds.y, maxBounds.y);

        return targetPosition;
    }
}

