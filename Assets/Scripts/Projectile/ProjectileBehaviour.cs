using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
    private Rigidbody body;

    [Header("Projectile Properties")]
    [SerializeField] private float flightSpeed;

    [Header("ProjectileStatus")]
    [SerializeField] private Color actualColor;

    [Header("ProjectileComponent")]
    [SerializeField] private GameObject splashEffectPrefab;

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
        body.AddForce(transform.forward * flightSpeed, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag ("Ground") || other.collider.CompareTag("Obstacle"))
        {
            Vector3 otherContact = other.contacts[0].point;
            Vector3 otherNormal = other.contacts[0].normal;

            GameObject actualSplashEffect = Instantiate(splashEffectPrefab, otherContact, Quaternion.identity);
            actualSplashEffect.transform.rotation = Quaternion.LookRotation(otherNormal);
            actualSplashEffect.GetComponent<SpriteRenderer>().color = actualColor;
            Destroy(gameObject);
        }
    }
}
