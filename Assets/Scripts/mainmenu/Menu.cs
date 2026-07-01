using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class Menu : MonoBehaviour
{
    [SerializeField]
    protected MenuData m_SoundData;

    protected void AssignSounds()
    {
        if (m_SoundData is null)
        {
            Debug.LogError("Menu: Menu sound data is not assigned.");
            return;
        }

        // Get the UIDocument component
        UIDocument uiDocument = GetComponent<UIDocument>();

        // Validate the UIDocument component
        if (uiDocument is null)
        {
            Debug.LogError("Menu: UIDocument component is not assigned.");
            return;
        }

        // Get the root visual element
        VisualElement root = uiDocument.rootVisualElement;

        // Validate the root
        if (root is null)
        {
            Debug.LogError("Menu: Root visual element not found.");
            return;
        }

        // Trickle down the sound to all buttons in the menu
        foreach (Button button in root.Query<Button>().ToList())
        {
            // Add hover sound
            if (m_SoundData.hoverSound != null)
            {
                button.RegisterCallback<MouseEnterEvent>(evt =>
                {
                    AudioSource.PlayClipAtPoint(m_SoundData.hoverSound, Camera.main.transform.position);
                });
            }

            // Add pressed sound
            if (m_SoundData.pressedSound != null)
            {
                button.clicked += () =>
                {
                    AudioSource.PlayClipAtPoint(m_SoundData.pressedSound, Camera.main.transform.position);
                };
            }
        }
    }
}
