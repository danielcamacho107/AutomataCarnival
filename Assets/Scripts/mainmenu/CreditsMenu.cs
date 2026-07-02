using UnityEngine;
using UnityEngine.UIElements;

public class CreditsMenu : Menu
{
    [SerializeField]
    private CreditsData m_Data;

    private VisualElement m_CreditsContainer;
    private VisualElement m_CollaboratorTemplate;

    private void GetTemplate()
    {
        // Get the UIDocument component
        UIDocument uiDocument = GetComponent<UIDocument>();
        if (uiDocument is null)
        {
            Debug.LogError("CreditsMenu: UIDocument component is not assigned.");
            return;
        }

        // Get the root visual element
        VisualElement root = uiDocument.rootVisualElement;
        if (root is null)
        {
            Debug.LogError("CreditsMenu: Root visual element not found.");
            return;
        }

        // Get the credits container and collaborator template
        m_CreditsContainer = root.Q<VisualElement>("CreditsContainer");
        m_CollaboratorTemplate = root.Q<VisualElement>("CollaboratorContainer");

        if (m_CreditsContainer is null)
        {
            Debug.LogError("CreditsMenu: Credits container not found in the UI.");
            return;
        }

        if (m_CollaboratorTemplate is null)
        {
            Debug.LogError("CreditsMenu: Collaborator container not found in the UI.");
        }
    }

    private VisualElement CloneElement(VisualElement original)
    {
        // Dynamically create a new instance of the original element's specific type
        VisualElement clone = (VisualElement)System.Activator.CreateInstance(original.GetType());
        clone.name = original.name;

        // Copy all assigned CSS classes to ensure the clone's styling matches the original
        foreach (var cls in original.GetClasses())
        {
            clone.AddToClassList(cls);
        }

        // Check if the element contains text (e.g., Label, Button) and copy the text content
        if (original is TextElement originalText && clone is TextElement cloneText)
        {
            cloneText.text = originalText.text;
        }

        // Recursively iterate over and clone all child elements, maintaining the hierarchy
        foreach (var child in original.Children())
        {
            clone.Add(CloneElement(child));
        }

        return clone;
    }

    private void CreateCollaboratorElements()
    {
        // Clear any existing collaborator elements from the credits container
        m_CreditsContainer.Clear();

        // Loop through the collaborators in the data and create UI elements for each
        for (int i = 0; i < m_Data.m_CreditsList.Count; i++)
        {
            Collaborator collaborator = m_Data.m_CreditsList[i];

            // Instantiate a new collaborator element from the template
            VisualElement collaboratorElement = CloneElement(m_CollaboratorTemplate);

            // Add the collaborator element to the credits container
            m_CreditsContainer.Add(collaboratorElement);
        }
    }

    private void FillCollaboratorData()
    {
        // Loop through the collaborators and fill in their data into the corresponding UI elements
        for (int i = 0; i < m_Data.m_CreditsList.Count; i++)
        {
            Collaborator collaborator = m_Data.m_CreditsList[i];

            // Get the corresponding collaborator element from the credits container
            VisualElement collaboratorElement = m_CreditsContainer.ElementAt(i);

            // Find the elements to be filled
            Label nameLabel = collaboratorElement.Q<Label>("Name");
            Label roleLabel = collaboratorElement.Q<Label>("Attribution");
            Button urlButton = collaboratorElement.Q<Button>("Link");

            // Validate the elements before filling in the data
            if (nameLabel is null)
            {
                Debug.LogError($"CreditsMenu: Name label not found for collaborator {collaborator.Name}.");
                continue;
            }

            if (roleLabel is null)
            {
                Debug.LogError($"CreditsMenu: Role label not found for collaborator {collaborator.Name}.");
                continue;
            }

            if (urlButton is null)
            {
                Debug.LogError($"CreditsMenu: URL button not found for collaborator {collaborator.Name}.");
                continue;
            }

            // Fill in the collaborator's data
            nameLabel.text = collaborator.Name;
            roleLabel.text = collaborator.Role;
            urlButton.clicked += () => Application.OpenURL(collaborator.URL);
        }
    }

    private void LinkBackButton()
    {
        // Get the UIDocument component
        UIDocument uiDocument = GetComponent<UIDocument>();
        if (uiDocument is null)
        {
            Debug.LogError("CreditsMenu: UIDocument component is not assigned.");
            return;
        }

        // Get the root visual element
        VisualElement root = uiDocument.rootVisualElement;
        if (root is null)
        {
            Debug.LogError("CreditsMenu: Root visual element not found.");
            return;
        }

        // Find the back button in the UI
        Button backButton = root.Q<Button>("BackToMenuButton");
        if (backButton is null)
        {
            Debug.LogError("CreditsMenu: Back button not found in the UI.");
            return;
        }

        // Link the back button to the CloseMenu method
        backButton.clicked += () => gameObject.SetActive(false);
    }

    private void Initialize()
    {
        GetTemplate();
        CreateCollaboratorElements();
        FillCollaboratorData();
        AssignSounds();
        LinkBackButton();
    }

    private void Awake()
    {
        Initialize();

        // Change the name of the GameObject to "CreditsMenu UI" for clarity in the hierarchy
        gameObject.name = "CreditsMenu UI";
    }

    private void OnEnable()
    {
        Initialize();
    }
}
