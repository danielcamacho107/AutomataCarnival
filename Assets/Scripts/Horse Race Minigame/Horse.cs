using UnityEngine;
using System.Collections;

public class Horse : MonoBehaviour
{
    // Flag to indicate whether the horse is currently moving
    bool m_IsMoving = false;

    private int m_SortingOrder;

    public int sortingOrder
    {
        get { return m_SortingOrder; }
    }

    // Public accessor for the sorting order
    public void UpdateSortingOrder()
    {
        m_SortingOrder = Random.Range(0, 101);
    }

    public void StopMoving()
    {
        // Reset the flag to indicate that the horse is not moving
        m_IsMoving = false;
    }

    public IEnumerator MoveToCoroutine(float distance, float movementTime)
    {
        // Validate the flag for movement
        if (m_IsMoving) { yield break; }

        // Set the flag to indicate that the horse is moving
        m_IsMoving = true;

        Vector3 initialPosition = transform.position;
        Vector3 targetPosition = initialPosition + new Vector3(distance, 0, 0);
        float elapsedTime = 0.0f;
        float normalizedTime = 0.0f;

        // Loop until the horse reaches the target position
        while (Vector3.Distance(transform.position, targetPosition) > 0.001f)
        {
            // Add the elapsed time since the last frame to the total elapsed time
            elapsedTime += Time.deltaTime;

            // Clamp it to the total time for the movement
            elapsedTime = Mathf.Min(elapsedTime, movementTime);

            // Normalize the time relative to the total time for the movement
            normalizedTime = elapsedTime / movementTime;

            // Lerp the position of the horse
            transform.position = Vector3.Lerp(initialPosition, targetPosition, normalizedTime);

            yield return null;
        }

        transform.position = targetPosition;

        // Reset the flag to indicate that the horse has stopped moving
        m_IsMoving = false;
    }
}
