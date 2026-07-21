using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BallLauncher : MonoBehaviour
{
    private Rigidbody rb;

    private bool finished = false;

    public float maxScoreSpeed = 3f;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Muro"))
        {
            ContactPoint contact = collision.contacts[0];
            rb.linearVelocity = Vector3.Reflect(rb.linearVelocity, contact.normal);
        }
    }

 private void OnTriggerEnter(Collider other)
{
    if (finished) return;

    if (other.CompareTag("ScoreZone"))
    {
        float speed = rb.linearVelocity.magnitude;

        // Solo anota si llega lo suficientemente lenta
        if (speed <= maxScoreSpeed)
        {
            finished = true;

            CanicasManager.Instance.AddPoint();
            CanicasManager.Instance.EndShot();

            Destroy(gameObject);
        }

        // Si va muy rápido, no hace nada y la pelota sigue su recorrido.
    }
    else if (other.CompareTag("LoseZone"))
    {
        finished = true;

        CanicasManager.Instance.EndShot();

        Destroy(gameObject);
    }
}
}
