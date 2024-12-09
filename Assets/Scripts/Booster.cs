using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script applies a continuous force to any Rigidbody that enters the trigger collider
// and plays a looping sound while the force is being applied.
public class ApplyForceAndPlaySoundOnCollision : MonoBehaviour
{
    [SerializeField]
    private float appliedForce = 10f;
    [SerializeField]
    private float minimumVelocityThreshold = 1f;
    [SerializeField]
    private AudioClip loopingAudioClip;

    private AudioSource audioPlayer;

    private void Start()
    {
        SetupAudioSource();
    }

    private void OnTriggerStay(Collider otherCollider)
    {
        Rigidbody otherRigidbody = GetRigidbodyFromCollider(otherCollider);
        if (otherRigidbody != null && ShouldApplyForce(otherRigidbody))
        {
            ApplyForceToRigidbody(otherRigidbody, otherCollider);
            PlayAudioIfNotPlaying();
        }
    }

    private void OnTriggerExit(Collider otherCollider)
    {
        // Stop playing the sound when the object exits the trigger zone
        audioPlayer.Stop();
    }

    private Rigidbody GetRigidbodyFromCollider(Collider collider)
    {
        return collider.gameObject.GetComponent<Rigidbody>();
    }

    private bool ShouldApplyForce(Rigidbody rigidbody)
    {
        return rigidbody.velocity.magnitude < minimumVelocityThreshold;
    }

    private void ApplyForceToRigidbody(Rigidbody rigidbody, Collider collider)
    {
        rigidbody.AddForce(appliedForce * collider.gameObject.transform.forward);
    }

    private void PlayAudioIfNotPlaying()
    {
        if (!audioPlayer.isPlaying)
        {
            audioPlayer.Play();
        }
    }

    private void SetupAudioSource()
    {
        audioPlayer = GetComponent<AudioSource>();
        if (audioPlayer == null)
        {
            audioPlayer = gameObject.AddComponent<AudioSource>();
            audioPlayer.clip = loopingAudioClip;
            audioPlayer.loop = true;
        }
    }
}