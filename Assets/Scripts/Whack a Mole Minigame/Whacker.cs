using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class Whacker : MonoBehaviour
{
    // Delegate for the event that is triggered when the score changes
    public delegate void WhackDataChangedHandler(uint score, uint timeRemaining);

    private Vector2 m_MousePosition;

    private uint m_Score;

    // Event that is triggered when the score changes
    private WhackDataChangedHandler m_WhackDataChanged;

    [SerializeField]
    [Tooltip("The starting time in seconds for the minigame.")]
    uint m_TimeRemaining = 300;

    private bool m_TimeTicking = true;

    public uint score
    {
        get
        {
            return m_Score;
        }
    }

    // Reference to the input actions class
    private WhackAMoleMinigame m_InputActions;

    public void SubscribeToWhackDataChanged(WhackDataChangedHandler handler)
    {
        m_WhackDataChanged += handler;
    }

    public void UnsubscribeFromWhackDataChanged(WhackDataChangedHandler handler)
    {
        m_WhackDataChanged -= handler;
    }

    private IEnumerator TickTimerCoorutine()
    {
        while(m_TimeRemaining > 0)
        {
            yield return new WaitForSeconds(1f);
            m_TimeTicking = true;
            m_TimeRemaining--;

            // Trigger the event to notify subscribers of the time change
            m_WhackDataChanged?.Invoke(m_Score, m_TimeRemaining);
        }
        m_TimeTicking = false;
        WhackMenuMgr mgr=FindAnyObjectByType<WhackMenuMgr>();
        mgr.OnGameEnd((int)score);
    }

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

            // Trigger the event to notify subscribers of the score change
            m_WhackDataChanged?.Invoke(m_Score, m_TimeRemaining);
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
    private void Start(){
        StartCoroutine(TickTimerCoorutine());
    }
    private void FixedUpdate()
    {
        SnapToMouse();
        
    }
}
