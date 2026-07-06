using UnityEngine;

public class BottleScript : MonoBehaviour
{
    [Header("Movimiento Botella")]
    public float maxAngle = 45f;
    public float speed = 2f;

    [Header("Medidor de Potencia")]
    public float powerSpeed = 2f;

    // 0 = Botella
    // 1 = Potencia
    // 2 = Finalizado
    private int phase = 0;

    private int precisionScore;
    private int powerScore;

    private float powerValue; // 0-1

    void Update()
    {
        switch (phase)
        {
            case 0:
                UpdateBottle();
                break;

            case 1:
                UpdatePowerMeter();
                break;
        }
    }

    void UpdateBottle()
    {
        float angle = Mathf.Sin(Time.time * speed) * maxAngle;
        transform.localRotation = Quaternion.Euler(0, 0, angle);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            float currentAngle = transform.localEulerAngles.z;

            if (currentAngle > 180)
                currentAngle -= 360;

            precisionScore = CalculateScore(Mathf.Abs(currentAngle), maxAngle);

            Debug.Log("Precisión: " + precisionScore);

            phase = 1;
        }
    }

    void UpdatePowerMeter()
    {
        // Valor entre 0 y 1
        powerValue = (Mathf.Sin(Time.time * powerSpeed) + 1f) / 2f;

        // Aquí puedes mover un Slider o una barra
        Debug.Log($"Potencia: {powerValue:F2}");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            powerScore = CalculateScore(Mathf.Abs(powerValue - 0.5f), 0.5f);

            Debug.Log("Potencia Score: " + powerScore);

            phase = 2;

            Debug.Log("----------------");
            Debug.Log("Precisión: " + precisionScore);
            Debug.Log("Potencia : " + powerScore);
        }
    }

    int CalculateScore(float distance, float maxDistance)
    {
        float normalized = distance / maxDistance;

        if (normalized <= 0.2f) return 5;
        if (normalized <= 0.4f) return 4;
        if (normalized <= 0.6f) return 3;
        if (normalized <= 0.8f) return 2;

        return 1;
    }

    public void Restart()
    {
        phase = 0;
        precisionScore = 0;
        powerScore = 0;
        powerValue = 0;

        transform.localRotation = Quaternion.identity;
    }
}