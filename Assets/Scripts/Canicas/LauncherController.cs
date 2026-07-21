using UnityEngine;
using UnityEngine.UI;

public class LauncherController : MonoBehaviour
{
    [Header("Movimiento")]
    public Transform leftPoint;
    public Transform rightPoint;
    public float moveSpeed = 3f;

    [Header("Disparo")]
    public GameObject ballPrefab;
    public Transform shootPoint;

    [Header("Potencia")]
    public Slider powerSlider;
    public float sliderSpeed = 1.5f;
    public float minForce = 5f;
    public float maxForce = 20f;

    private bool movingRight = true;

    // 0 = Moviendo
    // 1 = Potencia
    // 2 = Disparado
    private int phase = 0;

    private bool sliderUp = true;

    void Update()
    {
        switch (phase)
        {
            case 0:
                MoveLauncher();

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    phase = 1;
                }
                break;

            case 1:
                UpdateSlider();

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    Shoot();
                    phase = 2;
                }
                break;
        }
    }

   void MoveLauncher()
{
    Transform target = movingRight ? rightPoint : leftPoint;

    Vector3 newPosition = Vector3.MoveTowards(
        transform.position,
        new Vector3(target.position.x, transform.position.y, transform.position.z),
        moveSpeed * Time.deltaTime);

    transform.position = newPosition;

    if (Mathf.Abs(transform.position.x - target.position.x) < 0.01f)
    {
        movingRight = !movingRight;
    }
}

    void UpdateSlider()
    {
        if (sliderUp)
        {
            powerSlider.value += sliderSpeed * Time.deltaTime;

            if (powerSlider.value >= 1)
                sliderUp = false;
        }
        else
        {
            powerSlider.value -= sliderSpeed * Time.deltaTime;

            if (powerSlider.value <= 0)
                sliderUp = true;
        }
    }

    void Shoot()
    {
        float force = Mathf.Lerp(minForce, maxForce, powerSlider.value);

        GameObject ball = Instantiate(ballPrefab, shootPoint.position, Quaternion.identity);

        Rigidbody rb = ball.GetComponent<Rigidbody>();

        rb.linearVelocity = Vector3.up * force;
        // Si tu Unity no tiene linearVelocity:
        // rb.velocity = Vector3.up * force;
    }
    public void ResetLauncher()
{
    phase = 0;

    powerSlider.value = 0;
    sliderUp = true;
}
}