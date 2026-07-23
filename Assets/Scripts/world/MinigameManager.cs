using UnityEngine;

public class MinigameManager : MonoBehaviour
{
    // Instance of the singleton
    private static MinigameManager m_Instance;

    // Public property to access the singleton instance
    public static MinigameManager instance
    {
        get
        {
            if (m_Instance is null) { m_Instance = new MinigameManager(); }

            return m_Instance;
        }
    }

    public void EndMinigame()
    {

    }
}
