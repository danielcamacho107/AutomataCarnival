using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TargetSpawner : MonoBehaviour
{
    [SerializeField]
    private List<TargetSpawn> m_TargetSpawns;

    [SerializeField]
    [Tooltip("Minimum spawn interval in seconds.")]
    [Range(0f, 1f)]
    private float m_SpawnIntervalMin = 2.0f;

    [SerializeField]
    [Tooltip("Maximum spawn interval in seconds.")]
    [Range(1f, 10f)]
    private float m_SpawnIntervalMax = 5.0f;

    [SerializeField]
    [Tooltip("Direction in which targets will move when spawned.")]
    private Vector3 m_SpawnDirection;

    [SerializeField]
    [Tooltip("Prefab of the target to be spawned.")]
    private GameObject m_TargetPrefab;

    [SerializeField]
    [Tooltip("Multiplier for the speed of spawned targets.")]
    [Range(0f, 2f)]
    private float m_SpeedMultiplierStep = 0.1f;

    // Current speed multiplier for spawned targets
    static private float m_SpeedMultiplier = 1.0f;

    // The total probability of spawning targets, calculated as the sum of all individual spawn probabilities
    private float m_SpawnProbabilities;

    // Flag to indicate whether the spawner is currently in the process of spawning a target
    private bool m_Spawning;

    public void OnTargetHit(Target target)
    {
        m_SpeedMultiplier += m_SpeedMultiplierStep;
    }

    private int GetRandomTargetIndex()
    {
        // Roll a random value between 0 and the total spawn probabilities
        float randomValue = Random.Range(0f, m_SpawnProbabilities);

        // Iterate through the target spawns to find which one corresponds to the random value
        foreach (TargetSpawn spawn in m_TargetSpawns)
        {
            if (randomValue < spawn.spawnProbability)
            {
                return m_TargetSpawns.IndexOf(spawn);
            }

            randomValue -= spawn.spawnProbability;
        }

        // In case of an error, return the last index
        Debug.LogError("Error in GetRandomTargetIndex: random value exceeds total spawn probabilities.");
        return m_TargetSpawns.Count - 1;
    }

    private IEnumerator SpawnTargetsCoroutine()
    {
        // Set the spawning flag to true to prevent multiple concurrent spawns
        m_Spawning = true;

        // Wait for a random interval before spawning the next target
        float spawnInterval = Random.Range(m_SpawnIntervalMin, m_SpawnIntervalMax);
        yield return new WaitForSeconds(spawnInterval);

        // Select a random target spawn based on the defined probabilities
        TargetSpawn selectedTarget = m_TargetSpawns[GetRandomTargetIndex()];

        // Instantiate the target prefab
        GameObject targetInstance = Instantiate(m_TargetPrefab, null);

        // Set the game object name
        targetInstance.name = $"Target_{selectedTarget.targetData.gameObjectName}";

        // Set the target's position to the spawner's position
        targetInstance.transform.position = transform.position;

        // Get the Target component
        Target targetComponent = targetInstance.GetComponent<Target>();

        // Validate it
        if (targetComponent is null)
        {
            Debug.LogError("The target prefab does not have a Target component.");
        }

        // Set the target data and direction
        targetComponent.SetDirection(m_SpawnDirection);
        targetComponent.SetData(selectedTarget.targetData);
        targetComponent.SetSpeedMultiplier(m_SpeedMultiplier);
        targetComponent.SubscribeToHit(OnTargetHit);

        // Set the spawning flag to false to allow the next spawn
        m_Spawning = false;
    }

    private void Initialize()
    {
        if (m_TargetPrefab == null)
        {
            Debug.LogError("Target prefab is not assigned.");
            return;
        }

        if (m_TargetSpawns == null || m_TargetSpawns.Count == 0)
        {
            Debug.LogError("No target spawns assigned.");
            return;
        }

        // Calculate the total spawn probabilities
        m_SpawnProbabilities = 0f;
        foreach (TargetSpawn spawn in m_TargetSpawns)
        {
            m_SpawnProbabilities += spawn.spawnProbability;
        }
    }

    private void Awake()
    {
        Initialize();
    }

    private void FixedUpdate()
    {
        if (!m_Spawning)
        {
            StartCoroutine(SpawnTargetsCoroutine());
        }
    }
}
