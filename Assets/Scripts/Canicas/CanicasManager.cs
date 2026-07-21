using UnityEngine;
using TMPro;

public class CanicasManager : MonoBehaviour
{
    public static CanicasManager Instance;

    public LauncherController launcher;

    public TMP_Text scoreText;
    public TMP_Text attemptsText;
    public TMP_Text resultText;

    private int score = 0;
    private int attempts = 0;
    private const int maxAttempts = 3;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UpdateUI();
    }

    public void AddPoint()
    {
        score++;
        UpdateUI();
    }

    public void EndShot()
    {
        attempts++;
        UpdateUI();

        if (attempts >= maxAttempts+1)
        {
            EndGame();
        }
        else
        {
            launcher.ResetLauncher();
        }
    }

    void UpdateUI()
    {
        scoreText.text = "Puntos: " + score;
        attemptsText.text = "Intentos: " + attempts + "/" + maxAttempts;
    }

    void EndGame()
    {
        if (score >= 2)
            resultText.text = "¡Ganaste!";
        else
            resultText.text = "¡Perdiste!";
    }
}