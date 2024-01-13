using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothness = 5f;

    void Update()
    {
        if (target != null)
        {
            // Keep the current camera position's Y component
            float currentY = transform.position.y;

            // Calculate the target position with the same Y component
            Vector3 targetPosition = new Vector3(target.position.x, currentY, transform.position.z);

            // Smoothly interpolate between the current position and the target position
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * smoothness);
        }
        else
        {
            Debug.LogWarning("CameraFollow: Target not assigned.");
        }
    }
}
