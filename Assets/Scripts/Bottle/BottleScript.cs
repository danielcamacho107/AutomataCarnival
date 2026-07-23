using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BottleScript : MonoBehaviour
{
    [Header("Movimiento Botella")]
    public float maxAngle = 45f;
    public float speed = 2f;

    [Header("Medidor de Potencia")]
    public float powerSpeed = 2f;
    public Slider powerSlider;

    [Header("Intentos")]
    public int maxShots = 3;

    // 0 = Apuntar
    // 1 = Potencia
    // 2 = Finalizado
    private int phase = 0;

    private int precisionScore;
    private int powerScore;

    private float powerValue;
    [SerializeField] private GameObject UiMenupanl;

    private float precisionPercent;
    private float powerPercent;
    [SerializeField] private TextMeshProUGUI ResultText;

    private int currentShot = 0;
    private int hits = 0;

    void Start()
    {
        if (powerSlider != null)
        {
            powerSlider.minValue = 0;
            powerSlider.maxValue = 1;
            powerSlider.value = 0;
        }
    }

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

            case 2:
                   EndGame();

               /*if (Input.GetKeyDown(KeyCode.R))
                {
                   // RestartGame();
                }*/
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

            precisionPercent = 1f - (Mathf.Abs(currentAngle) / maxAngle);

            Debug.Log("Precisión: " + precisionScore);

            phase = 1;
        }
    }

    void UpdatePowerMeter()
    {
        powerValue = (Mathf.Sin(Time.time * powerSpeed) + 1f) / 2f;

        if (powerSlider != null)
            powerSlider.value = powerValue;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            powerScore = CalculateScore(Mathf.Abs(powerValue - 0.5f), 0.5f);

            powerPercent = 1f - (Mathf.Abs(powerValue - 0.5f) / 0.5f);

            float hitChance = (precisionPercent * 0.7f) + (powerPercent * 0.3f);

            bool hit = Random.value <= hitChance;

            currentShot++;

            Debug.Log("----------------------------");
            Debug.Log("Tiro #" + currentShot);
            Debug.Log("Precisión: " + precisionScore);
            Debug.Log("Potencia : " + powerScore);
            Debug.Log("Probabilidad: " + Mathf.RoundToInt(hitChance * 100) + "%");

            if (hit)
            {
                hits++;
                Debug.Log(" ¡ACERTASTE!");
            }
            else
            {
                Debug.Log(" FALLASTE");
            }

            if (currentShot >= maxShots)
            {
                phase = 2;

                Debug.Log("========================");
                Debug.Log("FIN DEL JUEGO");
                Debug.Log("Aciertos: " + hits + " / " + maxShots);
                Debug.Log("Presiona R para reiniciar.");
            }
            else
            {
                RestartShot();
            }
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

    void RestartShot()
    {
        phase = 0;

        precisionScore = 0;
        powerScore = 0;
        powerValue = 0;

        transform.localRotation = Quaternion.identity;

        if (powerSlider != null)
            powerSlider.value = 0;
    }

    public void RestartGame()
    {
        currentShot = 0;
        hits = 0;

        RestartShot();

        Debug.Log("Juego reiniciado.");
    }
    public void EndGame()
{
    Time.timeScale = 0f;

    if (hits >= 2)
    {
        ResultText.text = "¡You Win!";
    }
    else
    {
        ResultText.text = "¡You Lose!";
    }
    UiMenupanl.SetActive(true);
}
}