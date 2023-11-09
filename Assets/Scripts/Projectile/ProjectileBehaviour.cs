using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
    private Rigidbody body;

    [Header("Projectile Properties")]
    [SerializeField] private float flightSpeed;

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
        body.AddForce(transform.forward * flightSpeed, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Ground") || other.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
        }
    }
}
