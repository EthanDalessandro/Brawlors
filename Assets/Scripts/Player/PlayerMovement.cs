using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player Values")]
    [SerializeField] private Vector2 moveDirection;
    [SerializeField] private Vector3 dir;
    [SerializeField] private Vector3 target;

    [Header("PlayerStatus")]
    [SerializeField] private bool shooting;

    [Header("PlayerProperties")]
    [SerializeField, Range(0, 100)] private float moveSpeed;
    private float moveSpeedOrigin;
    [SerializeField, Range(0, 100)] private float moveSpeedWhileShooting;
    [SerializeField, Range(0, 100)] private float rotateSpeed;
    [SerializeField, Range(0, 100)] private float jumpForce;
    [SerializeField] private float fireRate;
    private float shootDelay;

    [Header("Player Component")]
    [SerializeField] private GameObject projectilePrefab;
    private Rigidbody body;

    private void Awake()
    {
        moveSpeedOrigin = moveSpeed;
        body = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        PlayerMove();
        PlayerRotationAndAim();
        Shooting();
        Debug.DrawRay(transform.position, target, Color.yellow);    
    }

    private void Shooting()
    {
        if (shooting)
        { 
            if(moveSpeed != moveSpeedWhileShooting)
            {
                StartCoroutine(moveSpeedSlowing());
            }
            shootDelay += Time.fixedDeltaTime;
            if (shootDelay >= fireRate)
            {
                GameObject actualProjectile = Instantiate(projectilePrefab, transform.position, Quaternion.LookRotation(target));
                shootDelay = 0;
            }
        }
        else
        {
            shootDelay += Time.fixedDeltaTime;
            shootDelay = Mathf.Clamp(shootDelay, 0, fireRate);
        }
    }

    private void PlayerRotationAndAim()
    {
        RaycastHit rayHit;
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out rayHit) && (rayHit.transform.CompareTag("Ground") || (rayHit.transform.CompareTag("Obstacle"))))
        {
            target = rayHit.point - transform.position;
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
        if (context.performed)
        {
            moveDirection = context.ReadValue<Vector2>();
        }
        else
        {
            moveDirection = Vector2.zero;
        }
    }

    public void Shooting(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            shooting = true;
        }
        else
        {
            shooting = false;
        }
    }

    public IEnumerator moveSpeedSlowing()
    {
        moveSpeed = moveSpeedWhileShooting;
        yield return new WaitForSeconds(0.5f);
        moveSpeed = moveSpeedOrigin;
    }
}
