using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;


public class WhackAMoleMinigameHUD : MonoBehaviour
{
    // Reference to the UIDocument component
    private UIDocument m_UIDocument;

    private Label m_ScoreCounter;
    private Label m_TimeCounter;

    private string GenerateTimeString(uint time)
    {
        uint minutes = time / 60u;
        uint remainingSeconds = time % 60u;

        string minutesString;
        string secondsString;

        // Add padding when necessary
        minutesString = minutes < 10 ? "0" + minutes.ToString() : minutes.ToString();
        secondsString = remainingSeconds < 10 ? "0" + remainingSeconds.ToString() : remainingSeconds.ToString();

        return $"{minutesString:D2}:{secondsString:D2}";
    }


    private void UpdateHUD(uint score, uint timeRemaining)
    {
        m_ScoreCounter.text = score.ToString();
        m_TimeCounter.text = GenerateTimeString(timeRemaining);
    }

    private void GetLabels()
    {
        VisualElement root = m_UIDocument.rootVisualElement;

        m_ScoreCounter = root.Q<Label>("ScoreCounter");
        m_TimeCounter = root.Q<Label>("TimeCounter");
    }

    private void ValidateReferences()
    {
        if (m_UIDocument is null)
        {
            Debug.LogError("UIDocument reference is missing on " + gameObject.name);
        }

        if (m_ScoreCounter is null)
        {
            Debug.LogError("Score Label reference is missing on " + gameObject.name);
        }

        if (m_TimeCounter is null)
        {
            Debug.LogError("Time Label reference is missing on " + gameObject.name);
        }
    }

    private void Initialize()
    {
        m_UIDocument = GetComponent<UIDocument>();

        GetLabels();

        // Look for the gunner and subscribe to its event
        Whacker whacker = FindFirstObjectByType<Whacker>();

        // Validate that the gunner was found before trying to subscribe to its event
        if (whacker is null)
        {
            Debug.LogError("Whacker not found in the scene.");
        }

        else
        {
            whacker.SubscribeToWhackDataChanged(UpdateHUD);
        }
    }

    private void Awake()
    {
        Initialize();
        ValidateReferences();
    }
}
