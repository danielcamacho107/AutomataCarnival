using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Whacker : MonoBehaviour
{
    private Vector2 m_MousePosition;

    private uint m_Score;

    public uint score
    {
        get
        {
            return m_Score;
        }
    }

    // Reference to the input actions class
    private WhackAMoleMinigame m_InputActions;
    private void SnapToMouse()
    {
        // Get the mouse position in world space
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(m_MousePosition);

        // Set the z position to 0 to keep the gun on the same plane
        mousePos.z = 0;

        // Move the gun to the mouse position
        transform.position = mousePos;
    }

    private void Whack()
    {
        // Scan for targets in range and hit them
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.zero);
        if (hit.collider != null && hit.collider.TryGetComponent<Mole>(out Mole mole))
        {
            mole.Hit();
            m_Score += mole.score;
        }
    }

    private void Initialize()
    {
        // Initialize the input actions
        m_InputActions = new WhackAMoleMinigame();

        // Enable the input actions
        m_InputActions.Enable();

        // Subscribe to the mouse position action
        m_InputActions.WhackMinigame.Aim.performed += ctx => m_MousePosition = ctx.ReadValue<Vector2>();

        // Subscribe to the whack action
        m_InputActions.WhackMinigame.Whack.performed += ctx => Whack();
    }

    private void Awake()
    {
        Initialize();
    }

    private void FixedUpdate()
    {
        SnapToMouse();
    }
}
