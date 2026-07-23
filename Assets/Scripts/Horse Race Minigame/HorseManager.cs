using UnityEngine;
using System.Collections.Generic;

public class HorseManager : MonoBehaviour
{
    public delegate void GoalReached(Horse winner);

    private static HorseManager m_Instance;

    public static HorseManager instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = FindAnyObjectByType<HorseManager>();
                if (m_Instance == null)
                {
                    GameObject obj = new GameObject("HorseManager");
                    m_Instance = obj.AddComponent<HorseManager>();
                }
            }

            return m_Instance;
        }
    }

    private List<Horse> m_Horses = new List<Horse>();

    [SerializeField]
    private float m_HorseMovementTime = 5.0f;

    [SerializeField]
    private uint m_HorseMovementSteps = 10;

    private float m_XStart = -7f;

    [SerializeField]
    [Tooltip("The position on the x coordinate the horse needs to reach.")]
    private float m_XGoal = 7f;

    private float m_DistancePerStep;
    private float m_TimePerStep;

    private bool m_Running;
    private bool m_Resetting;

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

        // Save the starting position of the horses
        m_XStart = m_Horses[0].transform.position.x;

        // Set the resetting flag to false
        m_Resetting = false;

        // Set the running flag to false
        StopRace();
    }

    public void RestartRace()
    {
        // Set the resetting flag to true
        m_Resetting = true;

        // Loop through each horse and stop their movement
        foreach (Horse horse in m_Horses)
        {
            horse.StopAllCoroutines();
            horse.StopMoving();
            horse.StartCoroutine(horse.MoveToCoroutine(m_XStart - horse.transform.position.x, m_TimePerStep));
        }

        // Set the running flag to false
        StopRace();
    }

    public void StartRace()
    {
        // Set the running flag to true
        m_Running = true;
    }

    public void StopRace()
    {
        // Set the running flag to false
        m_Running = false;
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

    private void CheckForWinner()
    {
        // Check if any horse has reached the goal
        foreach (Horse horse in m_Horses)
        {
            if (horse.transform.position.x >= m_XGoal)
            {
                // Stop the race
                StopRace();

                // Invoke the goal reached event
                m_GoalReached?.Invoke(horse);
                break;
            }
        }
    }

    private void Awake()
    {
        if (m_Instance != null && m_Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        m_Instance = this;
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
        if (m_Running)
        {
            SortHorses();
            MoveHorses();
            CheckForWinner();
        }

        if (m_Resetting)
        {
            bool allReset = true;
            // Once all horses have been reset, set the resetting flag to false
            foreach (Horse horse in m_Horses)
            {
                if (Mathf.Abs(horse.transform.position.x - m_XStart) > 0.001f)
                {
                    allReset = false;
                    break;
                }
            }

            if (allReset)
            {
                m_Resetting = false;
            }
        }
    }
}
