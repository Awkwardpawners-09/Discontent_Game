using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    public Transform head; // Attach the main camera or head Transform here.

    private Rigidbody rb;
    private Vector3 direction;
    public float playerSpeed = 5.0f;
    public float playerAcceleration = 2.0f;
    public float jumpForce = 6.0f;

    // Head Bob Variables
    public bool enableHeadBob = true;
    public Transform joint; // Assign the same Transform as 'head' or a child object of it.
    public float bobSpeed = 10f;
    public Vector3 bobAmount = new Vector3(0.15f, 0.05f, 0f);
    private Vector3 jointOriginalPos;
    private float bobTimer = 0;

    // Footstep Sound Variables
    public AudioClip[] footstepSounds;
    public float footstepInterval = 0.5f;
    public float footstepVolume = 0.7f;
    private AudioSource footstepAudioSource;
    private float footstepTimer = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (joint == null)
        {
            Debug.LogError("Joint (head bobbing target) not assigned!");
            return;
        }
        jointOriginalPos = joint.localPosition;

        // Initialize AudioSource for footsteps
        footstepAudioSource = gameObject.AddComponent<AudioSource>();
        footstepAudioSource.loop = false;
        footstepAudioSource.playOnAwake = false;
    }

    void Update()
    {
        // Movement
        direction = Input.GetAxisRaw("Horizontal") * head.right + Input.GetAxisRaw("Vertical") * head.forward;
        rb.velocity = Vector3.Lerp(rb.velocity, direction.normalized * playerAcceleration
            + rb.velocity.y * Vector3.up, playerAcceleration * Time.deltaTime);

        // Head Bobbing
        if (enableHeadBob)
        {
            HeadBob();
        }

        // Footstep Sounds
        HandleFootsteps();
    }

    private void HeadBob()
    {
        bool isMoving = Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0;

        if (isMoving)
        {
            bobTimer += Time.deltaTime * bobSpeed;
            joint.localPosition = new Vector3(
                jointOriginalPos.x + Mathf.Sin(bobTimer) * bobAmount.x,
                jointOriginalPos.y + Mathf.Sin(bobTimer) * bobAmount.y,
                jointOriginalPos.z
            );
        }
        else
        {
            bobTimer = 0;
            joint.localPosition = Vector3.Lerp(joint.localPosition, jointOriginalPos, Time.deltaTime * bobSpeed);
        }
    }

    private void HandleFootsteps()
    {
        bool isMoving = Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0;

        if (isMoving)
        {
            footstepTimer += Time.deltaTime;

            if (footstepTimer >= footstepInterval && footstepSounds.Length > 0)
            {
                // Play a random footstep sound
                int randomIndex = Random.Range(0, footstepSounds.Length);
                footstepAudioSource.clip = footstepSounds[randomIndex];
                footstepAudioSource.volume = footstepVolume;
                footstepAudioSource.Play();

                // Reset the footstep timer
                footstepTimer = 0f;
            }
        }
        else
        {
            footstepTimer = 0f;
        }
    }
}
