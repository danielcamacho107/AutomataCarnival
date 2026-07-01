using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using System.Collections.Generic;

[RequireComponent(typeof(UIDocument))]
public class PauseMenu : Menu
{
    // Delegate for the continue action, allowing external subscription to the continue event.
    public delegate void OnContinue();

    [SerializeField]
    private int m_MainMenuIndex = 0;

    private UIDocument m_UIDoc;

    private Button m_ContinueButton;
    private Button m_ExitToMenuButton;

    private OnContinue m_OnContinue;

    public void SubscribeToContinue(OnContinue continueAction)
    {
        m_OnContinue += continueAction;
    }

    private void GetButtons()
    {
        if (!m_UIDoc)
        {
            Debug.LogError("PauseMenu: UIDocument component is not assigned.");
            return;
        }

        VisualElement root = m_UIDoc.rootVisualElement;

        m_ContinueButton = root.Query<Button>("ContinueButton");
        m_ExitToMenuButton = root.Query<Button>("ExitToMenuButton");

        if (m_ContinueButton is null || m_ExitToMenuButton is null)
        {
            Debug.LogError("PauseMenu: One or more buttons not found in the UI.");
            return;
        }
    }

    private void LinkActions()
    {
        if (m_ContinueButton != null)
        {
            m_ContinueButton.clicked += () => m_OnContinue?.Invoke();
        }
        if (m_ExitToMenuButton != null)
        {
            m_ExitToMenuButton.clicked += () => SceneManager.LoadScene(m_MainMenuIndex);
        }
    }

    private void Initialize()
    {
        m_UIDoc = GetComponent<UIDocument>();

        if (!m_UIDoc)
        {
            Debug.LogError("MainMenu: No UIDocument component found on the GameObject.");
        }

        GetButtons();
        LinkActions();
        AssignSounds();
    }

    private void Awake()
    {
        Initialize();
    }
}
