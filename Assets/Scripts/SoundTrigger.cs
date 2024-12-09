using UnityEngine;

public class PlaySoundOnTrigger : MonoBehaviour
{
    public AudioClip triggerSound; // Assign the audio clip in the Inspector
    private AudioSource audioSource;

    private void Start()
    {
        // Get the AudioSource component attached to this GameObject
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            // If AudioSource is not found, add one
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Assign the audio clip to the AudioSource
        audioSource.clip = triggerSound;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the trigger has been entered by another GameObject
        // Play the sound if a GameObject with a Rigidbody enters the trigger
        if (other.GetComponent<Rigidbody>() != null)
        {
            audioSource.Play();
        }
    }
}
