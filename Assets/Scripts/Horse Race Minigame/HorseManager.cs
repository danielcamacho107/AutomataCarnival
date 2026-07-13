using UnityEngine;
using System.Collections.Generic;

public class HorseManager : MonoBehaviour
{
    public delegate void GoalReached(Horse winner);

    private HorseManager m_Instance;

    public HorseManager instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = new HorseManager();
            }

            return m_Instance;
        }
    }

    private List<Horse> m_Horses = new List<Horse>();

    [SerializeField]
    private float m_HorseMovementTime = 5.0f;

    [SerializeField]
    private uint m_HorseMovementSteps = 10;

    [SerializeField]
    [Tooltip("The position on the x coordinate the horse needs to reach.")]
    private float m_XGoal = 9f;

    private float m_DistancePerStep;
    private float m_TimePerStep;

    private GoalReached m_GoalReached;

    public void SubscribeToGoalReached(GoalReached callback)
    {
        m_GoalReached += callback;
    }

    public void UnsubscribeFromGoalReached(GoalReached callback)
    {
        m_GoalReached -= callback;
    }

    private void Initialize()
    {
        // Get the horses on the scene
        foreach (Horse horse in FindObjectsByType<Horse>(FindObjectsSortMode.None))
        {
            m_Horses.Add(horse);
        }

        // Calculate the max distance per step based on the goal and number of steps
        m_DistancePerStep = m_XGoal / m_HorseMovementSteps;

        // Calculate the time per step based on the total movement time and number of steps
        m_TimePerStep = m_HorseMovementTime / m_HorseMovementSteps;
    }

    private void SortHorses()
    {
        // Update each horse's sorting order
        foreach (Horse horse in m_Horses)
        {
            horse.UpdateSortingOrder();
        }

        // Sort the horses based on their sorting order
        m_Horses.Sort((horse1, horse2) => horse1.sortingOrder.CompareTo(horse2.sortingOrder));
    }

    private void MoveHorses()
    {
        // Assign the max distance to the first horse
        m_Horses[0].StartCoroutine(m_Horses[0].MoveToCoroutine(m_DistancePerStep, m_TimePerStep));

        // Loop through the rest of the horses and assign a random distance to each
        for (int i = 1; i < m_Horses.Count; i++)
        {
            float randomDistance = Random.Range(0f, m_DistancePerStep);

            m_Horses[i].StartCoroutine(m_Horses[i].MoveToCoroutine(randomDistance, m_TimePerStep));
        }
    }

    private void Awake()
    {
        Initialize();
    }

    private void Start()
    {
        // Validate that the horses have been initialized
        if (m_Horses.Count == 0)
        {
            Debug.LogError("No horses found in the scene. Please ensure that there are Horse objects present.");
            return;
        }
    }

    private void FixedUpdate()
    {
        SortHorses();
        MoveHorses();
    }
}
