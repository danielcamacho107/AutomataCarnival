using UnityEngine;
using UnityEngine.InputSystem;

public class BetHandler : MonoBehaviour
{
    // Reference to the input actions
    private HorseRaceMinigame m_InputActions;

    private Horse m_PredictedWinner;

    private void MoveHandler(InputAction.CallbackContext context)
    {
        // Move the position of the handler to match the mouse
        transform.position = context.ReadValue<Vector2>();
    }

    private void Initialize()
    {
        // Instantiate the input actions
        m_InputActions = new HorseRaceMinigame();

        // Enable the input actions
        m_InputActions.Enable();

        // Bind the actions
        m_InputActions.HorseMinigame.Select.performed += MoveHandler;
    }

    private void Awake()
    {
        Initialize();
    }
}
