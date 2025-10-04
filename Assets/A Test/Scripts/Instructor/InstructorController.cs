using UnityEngine;
using UnityEngine.AI;
using System;

public class InstructorController : MonoBehaviour
{
    [Header("References")]
    public Animator animator;
    public AudioSource audioSource;
    public NavMeshAgent agent;

    private bool isTalking = false;
    private bool walkingToTarget = false;
    private Transform currentTarget;

    public event Action OnReachedTarget; // StepManager subscribes

    void Update()
    {
        // Walking animation (only if actually moving)
        bool isMoving = agent.velocity.sqrMagnitude > 0.05f && !agent.isStopped;
        animator.SetBool("IsWalking", isMoving);

        // Talking animation
        if (isTalking && !audioSource.isPlaying)
        {
            animator.SetBool("IsTalking", false);
            isTalking = false;

            // After talking, start walking
            if (currentTarget != null)
            {
                agent.isStopped = false;
                agent.SetDestination(currentTarget.position);
                walkingToTarget = true;
            }
        }

        // Check arrival
        if (walkingToTarget && !agent.pathPending)
        {
            // Close enough?
            if (agent.remainingDistance <= agent.stoppingDistance + 0.05f)
            {
                // Agent is basically stopped
                if (agent.velocity.sqrMagnitude < 0.01f)
                {
                    walkingToTarget = false;
                    currentTarget = null;

                    agent.isStopped = true;   //  FORCE stop
                    agent.ResetPath();        // Clear path completely
                    animator.SetBool("IsWalking", false);

                    Debug.Log("Instructor arrived at target!");

                    OnReachedTarget?.Invoke();
                }
            }
        }
    }

    /// <summary>
    /// Plays audio and, optionally, walks to target after it finishes.
    /// </summary>
    public void PlayAudio(AudioClip clip, Transform targetAfter = null)
    {
        if (clip == null) return;

        audioSource.Stop();
        audioSource.clip = clip;
        audioSource.Play();

        animator.SetBool("IsTalking", true);
        isTalking = true;

        currentTarget = targetAfter;
    }
}

