using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
public class Target : MonoBehaviour
{
    // Delegate for handling target hit events
    public delegate void TargetHit(Target target);

    private TargetData m_Data;
    private Vector3 m_Direction;
    private float m_SpeedMultiplier;

    private SpriteRenderer m_SpriteRenderer;

    // Event that is invoked when the target is hit
    private TargetHit m_OnHit;

    // Accessor to get the target's score
    public uint score { get { return m_Data.pointValue; } }


    public void Hit()
    {
        // Invoke the hit event if there are subscribers
        m_OnHit?.Invoke(this);

        // Play the break sound
        AudioSource.PlayClipAtPoint(m_Data.breakSound, transform.position);

        // Destroy the target after it has been hit
        Destroy(gameObject);
    }

    public void SubscribeToHit(TargetHit callback)
    {
        m_OnHit += callback;
    }

    public void UnsubscribeFromHit(TargetHit callback)
    {
        m_OnHit -= callback;
    }

    public void SetSpeedMultiplier(float multiplier)
    {
        m_SpeedMultiplier = m_SpeedMultiplier == 0.0f ? multiplier : m_SpeedMultiplier;
    }

    public void SetData(TargetData data)
    {
        m_Data = m_Data is null ? data : m_Data;
    }

    public void SetDirection(Vector3 direction)
    {
        m_Direction = m_Direction == Vector3.zero ? direction : m_Direction;
    }

    public void FlipSprite()
    {
        if (!HasValidData()) { return; }

        // Flip the sprite based on the direction of movement
        m_SpriteRenderer.flipX = m_Direction.x < 0;
    }

    private Vector3 GetMoveDelta()
    {
        return m_Direction.normalized * m_Data.movementSpeed * m_SpeedMultiplier * Time.fixedDeltaTime;
    }

    private bool IsOffScreen()
    {
        // Check if the target is off-screen based on its position and the camera's viewport
        Vector3 viewportPos = Camera.main.WorldToViewportPoint(transform.position);
        return viewportPos.x < 0 || viewportPos.x > 1 || viewportPos.y < 0 || viewportPos.y > 1;
    }

    private bool HasValidData()
    {
        if (m_Data is null)
        {
            Debug.LogError("Target data is not assigned.");
            return false;
        }

        if (m_Direction == Vector3.zero)
        {
            Debug.LogError("Target direction is not assigned.");
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
        m_Direction = Vector3.zero;
        m_SpeedMultiplier = 0.0f;

        // Get the SpriteRenderer component
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Awake()
    {
        Initialize();
    }

    private void Start()
    {
        if (!HasValidData()) { return; }

        // Set the target's sprite based on the assigned data
        m_SpriteRenderer.sprite = m_Data.targetSprite;

        FlipSprite();
    }

    private void FixedUpdate()
    {
        if (!HasValidData()) { return; }

        // Move the target based on its speed and direction
        transform.position += GetMoveDelta();

        // Check if the target is off-screen and destroy it if it is
        if (IsOffScreen())
        {
            Destroy(gameObject);
        }
    }
}
