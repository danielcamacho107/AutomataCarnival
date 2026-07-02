using UnityEngine;
using UnityEngine.UIElements;


public class ShootingMinigameHUD : MonoBehaviour
{
    // Reference to the UIDocument component
    private UIDocument m_UIDocument;

    private Label m_ScoreLabel;
    private Label m_AmmoLabel;
    private Label m_AmmoReserveLabel;

    private void UpdateHUD(uint score, uint ammoInMagazine, uint magazineCapacity, uint ammoInReserve)
    {
        m_ScoreLabel.text = score.ToString();
        m_AmmoLabel.text = (ammoInMagazine < 10u ? "0" : "") + ammoInMagazine.ToString() + " / " + (magazineCapacity < 10u ? "0" : "") + magazineCapacity.ToString();
        m_AmmoReserveLabel.text = ammoInReserve.ToString();
    }

    private void GetLabels()
    {
        VisualElement root = m_UIDocument.rootVisualElement;

        m_ScoreLabel = root.Q<Label>("ScoreCounter");
        m_AmmoLabel = root.Q<Label>("MagazineLabel");
        m_AmmoReserveLabel = root.Q<Label>("ReserveLabel");
    }

    private void ValidateReferences()
    {
        if (m_UIDocument is null)
        {
            Debug.LogError("UIDocument reference is missing on " + gameObject.name);
        }

        if (m_ScoreLabel is null)
        {
            Debug.LogError("Score Label reference is missing on " + gameObject.name);
        }

        if (m_AmmoLabel is null)
        {
            Debug.LogError("Ammo Label reference is missing on " + gameObject.name);
        }

        if (m_AmmoReserveLabel is null)
        {
            Debug.LogError("Ammo Reserve Label reference is missing on " + gameObject.name);
        }
    }

    private void Initialize()
    {
        m_UIDocument = GetComponent<UIDocument>();

        GetLabels();

        // Look for the gunner and subscribe to its event
        Gunner gunner = FindFirstObjectByType<Gunner>();

        // Validate that the gunner was found before trying to subscribe to its event
        if (gunner is null)
        {
            Debug.LogError("Gunner not found in the scene.");
        }

        else
        {
            gunner.SubscribeToGunDataChanged(UpdateHUD);
        }
    }

    private void Awake()
    {
        Initialize();
        ValidateReferences();
    }
}
