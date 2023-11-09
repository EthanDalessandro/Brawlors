using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player Values")]
    [SerializeField] private Vector2 moveDirection;
    [SerializeField] private Vector3 dir;
    [SerializeField] private Vector3 target;

    [Header("PlayerProperties")]
    [SerializeField, Range(0,100)] private float moveSpeed;
    [SerializeField, Range(0,100)] private float rotateSpeed;
    [SerializeField, Range(0,100)] private float jumpForce;

    [Header("Player Component")]
    private Rigidbody body;

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        PlayerMove();
        PlayerRotation();

        RaycastHit rayHit;
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out rayHit) && (rayHit.transform.CompareTag("Ground") || (rayHit.transform.CompareTag("Obstacle"))))
        {
            target = rayHit.point - transform.position;
        }
        Debug.DrawRay(transform.position, target);
    }

    private void PlayerRotation()
    {
        RaycastHit rayHit;
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out rayHit) && (rayHit.transform.CompareTag("Ground") || (rayHit.transform.CompareTag("Obstacle"))))
        {
            dir = rayHit.point - transform.position;
            dir.y = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.fixedDeltaTime * rotateSpeed);
        }
    }

    private void PlayerMove()
    {
        body.velocity = new Vector3(moveDirection.x * moveSpeed, body.velocity.y, moveDirection.y * moveSpeed);
    }

    public void Movement(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            moveDirection = context.ReadValue<Vector2>();
        }
        else
        {
            moveDirection = Vector2.zero;
        }
    }
}
