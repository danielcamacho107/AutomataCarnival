using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using System.Collections.Generic;

[RequireComponent(typeof(UIDocument))]
public class MainMenu : Menu
{
    [SerializeField]
    private int m_NextSceneIndex = 1;

    [SerializeField]
    private GameObject m_CreditsPrefab;
    private GameObject m_CreditsInstance;

    private UIDocument m_UIDoc;

    private Button m_PlayButton;
    private Button m_CreditsButton;
    private Button m_QuitButton;

    private void GetButtons()
    {
        if (!m_UIDoc)
        {
            Debug.LogError("MainMenu: UIDocument component is not assigned.");
            return;
        }

        VisualElement root = m_UIDoc.rootVisualElement;

        m_PlayButton = root.Query<Button>("PlayButton");
        m_CreditsButton = root.Query<Button>("CreditsButton");
        m_QuitButton = root.Query<Button>("QuitButton");

        if (m_PlayButton is null || m_CreditsButton is null || m_QuitButton is null)
        {
            Debug.LogError("MainMenu: One or more buttons not found in the UI.");
        }
    }

    private void ValidateReferences()
    {
        if (m_CreditsPrefab is null)
        {
            Debug.LogError("MainMenu: Credits prefab is not assigned.");
        }
    }

    private void ShowCredits()
    {
        // Instantiate the credits prefab if it hasn't been created yet
        if (m_CreditsInstance is null)
        {
            m_CreditsInstance = Instantiate(m_CreditsPrefab);
        }

        // Enable the credits instance
        else { m_CreditsInstance.SetActive(true); }
    }

    private void LinkActions()
    {
        if (m_PlayButton != null)
        {
            m_PlayButton.clicked += () => SceneManager.LoadScene(m_NextSceneIndex);
        }
        if (m_CreditsButton != null)
        {
            m_CreditsButton.clicked += () => ShowCredits();
        }
        if (m_QuitButton != null)
        {
#if UNITY_EDITOR
            m_QuitButton.clicked += () => UnityEditor.EditorApplication.isPlaying = false;
#else 
            m_QuitButton.clicked += () => Application.Quit();
#endif

        }
    }

    private void Initialize()
    {
        m_UIDoc = GetComponent<UIDocument>();

        if (!m_UIDoc)
        {
            Debug.LogError("MainMenu: No UIDocument component found on the GameObject.");
        }

        ValidateReferences();
        GetButtons();
        LinkActions();
        AssignSounds();
    }

    private void Awake()
    {
        Initialize();
    }
}
