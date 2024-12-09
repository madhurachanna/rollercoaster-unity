using UnityEngine;

public class SmoothFollowCamera : MonoBehaviour
{
    public Transform target;          // The roller coaster car that the camera should follow
    public float smoothSpeed = 5.0f; // The speed at which the camera should follow the target
    public Vector3 localOffset = new Vector3(0.0f, 2.0f, -5.0f); // The local offset relative to the target
    public float rotationSpeed = 2.0f; // The speed at which the camera should rotate to match the car's rotation

    private void LateUpdate()
    {
        if (target == null)
        {
            Debug.LogWarning("Target not assigned for SmoothFollowCamera.");
            return;
        }

        // Calculate the desired position of the camera based on the target's position and local offset
        Vector3 desiredPosition = target.TransformPoint(localOffset);

        // Use Mathf.Lerp to smoothly interpolate between the current camera position and the desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        // Update the camera's position to the smoothed position
        transform.position = smoothedPosition;

        // Calculate the desired rotation of the camera based on the car's rotation
        Quaternion desiredRotation = Quaternion.LookRotation(target.forward, target.up);
        
        // Use Quaternion.Slerp to smoothly interpolate between the current camera rotation and the desired rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, rotationSpeed * Time.deltaTime);
    }
}
