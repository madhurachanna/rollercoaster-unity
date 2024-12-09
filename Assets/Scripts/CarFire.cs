using UnityEngine;
using System.Collections;

public class WildFireController : MonoBehaviour
{
    private ParticleSystem wildfireParticleSystem;
    private bool hasContactedEnd = false;
    public AudioClip triggerSound;
    public AudioClip triggerSound2;
    public AudioClip triggerSound3;
    private AudioSource audioSource;
    public GameObject engine;
    private SplineFollow script;
    private Rigidbody rb;
    public float jumpForce = 5f; // Adjust this value for the desired jump height
    public float jumpSidewaysForce = 2f; // Adjust this value for the desired sideways jump force
    public float jumpTorque = 1f; // Adjust this value for the desired flip torque

    private void Start()
    {
        script = engine.GetComponent<SplineFollow>();
        rb = engine.GetComponent<Rigidbody>();
        // Get the Particle System component from the prefab itself
        wildfireParticleSystem = GetComponent<ParticleSystem>();
        // Deactivate the Particle System initially
        wildfireParticleSystem.Stop();
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
        // Check if the collider's tag matches the desired object (e.g., "End")
        if (other.CompareTag("End") && !hasContactedEnd)
        {
            // Play the Particle System
            wildfireParticleSystem.Play();
            audioSource.Play();
            StartCoroutine(ActivateObjectsWithDelay());

            // Apply an upward force for jumping
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            // Apply a sideways force for a slight right jump
            rb.AddForce(Vector3.right * jumpSidewaysForce, ForceMode.Impulse);
            // Apply torque for a little flip
            rb.AddTorque(Vector3.forward * jumpTorque, ForceMode.Impulse);

            hasContactedEnd = true;

            // Debug statement
            Debug.Log("Particle System activated!");
        }
    }

    private IEnumerator ActivateObjectsWithDelay()
    {
        Debug.Log("Particle System activated!");
        script.isFollowingSpline = false;
        rb.Sleep();

        // Wait for a delay before playing the next audio clip
        yield return new WaitForSeconds(1.2f);

        // Play triggerSound2
        audioSource.clip = triggerSound2;
        audioSource.Play();

        // Wait for triggerSound2 to finish playing
        while (audioSource.isPlaying)
        {
            yield return null;
        }

        // Play triggerSound3
        audioSource.clip = triggerSound3;
        audioSource.Play();

        // Find all audio sources in the scene
        AudioSource[] audioSources = FindObjectsOfType<AudioSource>();

        // Turn off all audio sources except the current one
        foreach (AudioSource source in audioSources)
        {
            if (source != audioSource)
            {
                source.Stop();
            }
        }
    }
}
