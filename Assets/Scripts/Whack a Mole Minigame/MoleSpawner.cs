using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoleSpawner : MonoBehaviour
{
    [SerializeField]
    private List<MoleSpawn> m_MoleSpawns;

    [SerializeField]
    [Tooltip("Minimum spawn interval in seconds.")]
    [Range(0f, 1f)]
    private float m_SpawnIntervalMin = 2.0f;

    [SerializeField]
    [Tooltip("Maximum spawn interval in seconds.")]
    [Range(1f, 10f)]
    private float m_SpawnIntervalMax = 5.0f;

    [SerializeField]
    [Tooltip("Prefab of the mole to be spawned.")]
    private GameObject m_MolePrefab;

    // The total probability of spawning targets, calculated as the sum of all individual spawn probabilities
    private float m_SpawnProbabilities;

    // Flag to indicate whether the spawner has a mole currently spawned
    private bool m_IsSpawnerBusy;

    // Reference to the currently spawned mole
    private Mole m_CurrentMole;

    private int GetRandomMoleIndex()
    {
        // Roll a random value between 0 and the total spawn probabilities
        float randomValue = Random.Range(0f, m_SpawnProbabilities);

        // Iterate through the mole spawns to find which one corresponds to the random value
        foreach (MoleSpawn spawn in m_MoleSpawns)
        {
            if (randomValue < spawn.spawnProbability)
            {
                return m_MoleSpawns.IndexOf(spawn);
            }

            randomValue -= spawn.spawnProbability;
        }

        // In case of an error, return the last index
        Debug.LogError("Error in GetRandomMoleIndex: random value exceeds total spawn probabilities.");
        return m_MoleSpawns.Count - 1;
    }

    private void Initialize()
    {
        if (m_MolePrefab == null)
        {
            Debug.LogError("Mole prefab is not assigned.");
            return;
        }

        if (m_MoleSpawns == null || m_MoleSpawns.Count == 0)
        {
            Debug.LogError("No mole spawns assigned.");
            return;
        }

        // Calculate the total spawn probabilities
        m_SpawnProbabilities = 0f;
        foreach (MoleSpawn spawn in m_MoleSpawns)
        {
            m_SpawnProbabilities += spawn.spawnProbability;
        }

        // Spawn the first mole immediately
        GameObject moleObject = Instantiate(m_MolePrefab, transform.position, Quaternion.identity);
        m_CurrentMole = moleObject.GetComponent<Mole>();

        // Subscribe to the mole's OnMoleHit event to handle when the mole is hit
        m_CurrentMole.SubscribeToHit((Mole mole) =>
        {
            // Disable the mole when it is hit
            m_CurrentMole.gameObject.SetActive(false);
        });

        // Disable the mole initially until the first spawn interval has passed
        m_CurrentMole.gameObject.SetActive(false);
    }

    private IEnumerator SpawnMoleCoorutine()
    {
        // Set the flag to indicate that a spawner is currently busy
        m_IsSpawnerBusy = true;

        // Wait for the spawn interval to elapse
        yield return new WaitForSeconds(Random.Range(m_SpawnIntervalMin, m_SpawnIntervalMax));

        // Enable the mole
        m_CurrentMole.gameObject.SetActive(true);

        // Assign it its data
        m_CurrentMole.SetData(m_MoleSpawns[GetRandomMoleIndex()].moleData);

        // Wait for the mole's active time to elapse
        yield return new WaitForSeconds(m_CurrentMole.activeTime);

        // Disable the mole after its active time has elapsed
        m_CurrentMole.gameObject.SetActive(false);

        // Reset the flag to indicate that the spawner is no longer busy
        m_IsSpawnerBusy = false;
    }

    private void Awake()
    {
        Initialize();
    }

    private void FixedUpdate()
    {
        if (!m_IsSpawnerBusy)
        {
            StartCoroutine(SpawnMoleCoorutine());
        }
    }
}
