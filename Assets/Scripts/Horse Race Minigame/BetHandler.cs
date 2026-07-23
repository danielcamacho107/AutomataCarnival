using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class BetHandler : MonoBehaviour
{
    public delegate void BetWonHandler(bool betWon);
    private BetWonHandler m_BetWonCallback;

    // Reference to the input actions
    private HorseRaceMinigame m_InputActions;

    private Horse m_SelectedHorse;
    private Horse m_PredictedWinner;

    public void SubscribeToHorseSelected(BetWonHandler callback)
    {
        m_BetWonCallback += callback;
    }

    public void UnsubscribeFromHorseSelected(BetWonHandler callback)
    {
        m_BetWonCallback -= callback;
    }

    private void MoveHandler(InputAction.CallbackContext context)
    {
        // Move the object to the mouse position relative to the camera
        transform.position = Camera.main.ScreenToWorldPoint(context.ReadValue<Vector2>());
    }

    private void Bet()
    {
        // Skip the betting process if no horse is selected
        // or if there's already a predicted winner
        if (m_SelectedHorse == null || m_PredictedWinner != null)
        {
            return;
        }

        // Set the predicted winner to the selected horse
        m_PredictedWinner = m_SelectedHorse;

        // Start the horse race
        HorseManager.instance.StartRace();
    }

    private IEnumerator WaitToResetRaceCoroutine()
    {
        // Wait for a short duration before
        yield return new WaitForSeconds(1.0f);

        // Reset the race state in the Horse
        HorseManager.instance.RestartRace();
    }
    private void OnHorseReachedGoal(Horse winner)
    {
        m_BetWonCallback?.Invoke(m_PredictedWinner == winner);

        // Reset the predicted winner for the next race
        m_PredictedWinner = null;

        // Reset the selected horse for the next race
        m_SelectedHorse = null;

        // Reset the race state in the HorseManager
        StartCoroutine(WaitToResetRaceCoroutine());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the collided object has a Horse component
        if (collision.TryGetComponent(out Horse horse))
        {
            // Set the selected horse
            m_SelectedHorse = horse;
        }
    }

    private void Initialize()
    {
        // Instantiate the input actions
        m_InputActions = new HorseRaceMinigame();

        // Enable the input actions
        m_InputActions.Enable();

        // Bind the actions
        m_InputActions.HorseMinigame.Select.performed += MoveHandler;
        m_InputActions.HorseMinigame.Bet.performed += context => Bet();

        // Subscribe to the horse reached the finish line event
        HorseManager.instance.SubscribeToGoalReached(OnHorseReachedGoal);
    }

    private void Awake()
    {
        Initialize();
    }
}
