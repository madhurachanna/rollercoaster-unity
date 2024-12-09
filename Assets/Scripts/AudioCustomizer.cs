using UnityEngine;

public class AdjustAudioSpeedByObjectSpeed : MonoBehaviour
{
    [SerializeField, Range(0.1f, 2f)]
    private float minPlaybackSpeed = 0.5f;

    [SerializeField, Range(0.5f, 4f)]
    private float maxPlaybackSpeed = 2f;

    [SerializeField]
    private float maxSpeed = 35f;

    private AudioSource[] audioSources;
    private Rigidbody rigidbody;

    private void Awake()
    {
        audioSources = GetComponents<AudioSource>();
        rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        float speed = rigidbody.velocity.magnitude;
        float normalizedSpeed = Mathf.Clamp01(speed / maxSpeed);
        float mappedPlaybackSpeed = Mathf.Lerp(minPlaybackSpeed, maxPlaybackSpeed, normalizedSpeed);

        foreach (AudioSource audioSource in audioSources)
        {
            audioSource.pitch = mappedPlaybackSpeed;
        }
    }
}