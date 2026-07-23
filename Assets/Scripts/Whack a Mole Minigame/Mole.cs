using System.Collections;
using UnityEngine;

public class Mole : MonoBehaviour
{
    // Delegate for handling target hit events
    public delegate void MoleHit(Mole target);

    private MoleData m_Data;

    private SpriteRenderer m_SpriteRenderer;

    // Event that is invoked when the target is hit
    private MoleHit m_OnHit;

    // Accessor to get the target's score
    public uint score { get { return m_Data.pointValue; } }

    // Accessor to get the target's active time
    public float activeTime { get { return m_Data.activeTime; } }

    public void Hit()
    {
        // Invoke the hit event if there are subscribers
        m_OnHit?.Invoke(this);

        // Play the hit sound
        AudioSource.PlayClipAtPoint(m_Data.hitSound, transform.position);
    }

    public void SubscribeToHit(MoleHit callback)
    {
        m_OnHit += callback;
    }

    public void UnsubscribeFromHit(MoleHit callback)
    {
        m_OnHit -= callback;
    }

    public void SetData(MoleData data)
    {
        // Assign the data to the mole
        m_Data = data is null ? m_Data : data;

        // Validate the data and components before proceeding
        if (!HasValidData()) { return; }

        // Update the mole's sprite based on the assigned data
        m_SpriteRenderer.sprite = m_Data.moleSprite;

        // Update the game object's name to match the data's name
        gameObject.name = m_Data.gameObjectName;
    }

    private bool HasValidData()
    {
        if (m_Data is null)
        {
            Debug.LogError("Mole data is not assigned.");
            return false;
        }

        if (m_SpriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer component is missing.");
            return false;
        }

        return true;
    }

    private void Initialize()
    {
        m_Data = null;

        // Get the SpriteRenderer component
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Awake()
    {
        Initialize();
    }
}
