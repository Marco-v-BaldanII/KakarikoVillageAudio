using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Serialization;

public enum Surfaces
{
    GRASS,
    SAND,
    ROCK,
    WOOD
}

[Serializable] /* all its fields will appear as a dropdown in the editor */
public class FootsetpCollection
{
    [Serializable]
    public class Audio
    {
        public AudioClip clip;
        [Range(0, 1)] public float volume;
        [SerializeField, Range(0, 1)] public float volume_variation;

        public float VolumeVariation()
        {
            return 1f + UnityEngine.Random.Range(-volume_variation, 0);
        }

    }

    [Header("Grass")]
    [SerializeField] Audio[] grass_footsteps;
    [SerializeField] Audio[] grass_landing;
    [Header("Sand")]
    [SerializeField] Audio[] sand_footsteps;
    [SerializeField] Audio[] sand_landing;
    [Header("Rock")]
    [SerializeField] Audio[] rock_footsteps;
    [SerializeField] Audio[] rock_landing;
    [Header("Wood")]
    [SerializeField] Audio[] wood_footsteps;
    [SerializeField] Audio[] wood_landing;

    [Header("Congig")] /* 2 = twice as fast, an octave higher */
    [SerializeField, Range(1, 2)] private float pitch_variation;




    public static Surfaces surface;
    [Range(0, 1)] public float volume;

    public Audio GetFootstep()
    {
        Audio clip = null;
        switch (surface)
        {
            case Surfaces.GRASS:
                clip = grass_footsteps[UnityEngine.Random.Range(0, grass_footsteps.Length)];
                break;
            case Surfaces.SAND:
                clip = sand_footsteps[UnityEngine.Random.Range(0, sand_footsteps.Length)];
                break;
            case Surfaces.ROCK:
                clip = rock_footsteps[UnityEngine.Random.Range(0, rock_footsteps.Length)];
                break;
            case Surfaces.WOOD:
                clip = wood_footsteps[UnityEngine.Random.Range(0, wood_footsteps.Length)];
                break;

        }
        return clip;
    }


    public float PitchVariation()
    {
        return UnityEngine.Random.Range(1 / pitch_variation, pitch_variation);
    }

    public float VolumeVariation(Audio audio)
    {
        return UnityEngine.Random.Range(-audio.volume_variation, audio.volume_variation);
    }

}



// MoveBehaviour inherits from GenericBehaviour. This class corresponds to basic walk and run behaviour, it is the default behaviour.
public class MoveBehaviour : GenericBehaviour
{
    public float walkSpeed = 0.15f;                 // Default walk speed.
    public float runSpeed = 1.0f;                   // Default run speed.
    public float sprintSpeed = 2.0f;                // Default sprint speed.
    public float speedDampTime = 0.1f;              // Default damp time to change the animations based on current speed.
    public string jumpButton = "Jump";              // Default jump button.
    public float jumpHeight = 1.5f;                 // Default jump height.
    public float jumpInertialForce = 10f;          // Default horizontal inertial force when jumping.

    private float speed, speedSeeker;               // Moving speed.
    private int jumpBool;                           // Animator variable related to jumping.
    private int groundedBool;                       // Animator variable related to whether or not the player is on ground.
    private bool jump;                              // Boolean to determine whether or not the player started a jump.
    private bool isColliding;

    [SerializeField] private AudioSource m_PSpeedSource;
    private Coroutine m_PSSpeedCorroutine;

    // Boolean to determine if the player has collided with an obstacle.

    [SerializeField] FootsetpCollection footsteps;
    [SerializeField] AudioClip[] JumpScreams;
    [SerializeField] AnimationCurve curve;
    private AudioSource source;

    public Animator LinkAnim;

    public Surfaces surface;
    protected void Awake()
    {
        source = GetComponent<AudioSource>();
        base.Awake();
    }

    // Start is always called after any Awake functions.
    void Start()
    {
        // Set up the references.
        jumpBool = Animator.StringToHash("Jump");
        groundedBool = Animator.StringToHash("Grounded");
        behaviourManager.GetAnim.SetBool(groundedBool, true);
        LinkAnim.SetBool("Grounded", true);

        // Subscribe and register this behaviour as the default behaviour.
        behaviourManager.SubscribeBehaviour(this);
        behaviourManager.RegisterDefaultBehaviour(this.behaviourCode);
        speedSeeker = runSpeed;
    }

    // Update is used to set features regardless the active behaviour.
    void Update()
    {
        surface = FootsetpCollection.surface;
        // Get jump input.
        if (!jump && Input.GetButtonDown(jumpButton) && behaviourManager.IsCurrentBehaviour(this.behaviourCode) && !behaviourManager.IsOverriding())
        {
            jump = true;
        }

    }

    // LocalFixedUpdate overrides the virtual function of the base class.
    public override void LocalFixedUpdate()
    {
        // Call the basic movement manager.
        MovementManagement(behaviourManager.GetH, behaviourManager.GetV);

        // Call the jump manager.
        JumpManagement();
    }

    // Execute the idle and walk/run jump movements.
    void JumpManagement()
    {
        // Start a new jump.
        if (jump && !behaviourManager.GetAnim.GetBool(jumpBool) && behaviourManager.IsGrounded())
        {
            // Set jump related parameters.
            behaviourManager.LockTempBehaviour(this.behaviourCode);
            behaviourManager.GetAnim.SetBool(jumpBool, true);
            LinkAnim.SetBool("Jump", true);

            // Jump audi
            source.volume = 1;
            source.PlayOneShot(JumpScreams[UnityEngine.Random.Range(0,JumpScreams.Length)]);


            // Is a locomotion jump?
            if (behaviourManager.GetAnim.GetFloat(speedFloat) > 0.1)
            {
                // Temporarily change player friction to pass through obstacles.
                GetComponent<CapsuleCollider>().material.dynamicFriction = 0f;
                GetComponent<CapsuleCollider>().material.staticFriction = 0f;
                // Remove vertical velocity to avoid "super jumps" on slope ends.
                RemoveVerticalVelocity();
                // Set jump vertical impulse velocity.
                float velocity = 2f * Mathf.Abs(Physics.gravity.y) * jumpHeight;
                velocity = Mathf.Sqrt(velocity);
                behaviourManager.GetRigidBody.AddForce(Vector3.up * velocity, ForceMode.VelocityChange);
            }
        }
        // Is already jumping?
        else if (behaviourManager.GetAnim.GetBool(jumpBool))
        {
            // Keep forward movement while in the air.
            if (!behaviourManager.IsGrounded() && !isColliding && behaviourManager.GetTempLockStatus())
            {
                behaviourManager.GetRigidBody.AddForce(transform.forward * (jumpInertialForce * Physics.gravity.magnitude * sprintSpeed), ForceMode.Acceleration);
            }
            // Has landed?
            if ((behaviourManager.GetRigidBody.velocity.y < 0) && behaviourManager.IsGrounded())
            {
                PlayLanding();
                behaviourManager.GetAnim.SetBool(groundedBool, true);
                // Change back player friction to default.
                GetComponent<CapsuleCollider>().material.dynamicFriction = 0.6f;
                GetComponent<CapsuleCollider>().material.staticFriction = 0.6f;
                // Set jump related parameters.
                jump = false;
                behaviourManager.GetAnim.SetBool(jumpBool, false);
                LinkAnim.SetBool("Jump", false);
                behaviourManager.UnlockTempBehaviour(this.behaviourCode);
            }
        }
    }

    // Deal with the basic player movement

    //TODO Pedirle a Pau el update y el MovementManager
    void MovementManagement(float horizontal, float vertical)
    {
        // On ground, obey gravity.
        if (behaviourManager.IsGrounded())
            behaviourManager.GetRigidBody.useGravity = true;

        // Avoid takeoff when reached a slope end.
        else if (!behaviourManager.GetAnim.GetBool(jumpBool) && behaviourManager.GetRigidBody.velocity.y > 0)
        {
            RemoveVerticalVelocity();
        }

        // Call function that deals with player orientation.
        Rotating(horizontal, vertical);

        // Set proper speed.
        Vector2 dir = new Vector2(horizontal, vertical);
        speed = Vector2.ClampMagnitude(dir, 1f).magnitude;

        // This is for PC only, gamepads control speed via analog stick.
        speedSeeker += Input.GetAxis("Mouse ScrollWheel");
        speedSeeker = Mathf.Clamp(speedSeeker, walkSpeed, runSpeed);
        speed *= speedSeeker;



        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            speed = sprintSpeed;
            if (m_PSSpeedCorroutine != null) StopCoroutine(m_PSSpeedCorroutine);
            m_PSSpeedCorroutine = StartCoroutine(PSpeedAudioStart(1f));
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift)) /* Shift is not pressed */
        {

            if (m_PSSpeedCorroutine != null) StopCoroutine(m_PSSpeedCorroutine);
            m_PSSpeedCorroutine = StartCoroutine(PSpeedAudioEnd(0.5f));
            m_PSSpeedCorroutine = null; // must be set to null ?

        }

        LinkAnim.SetFloat("speed", speed, speedDampTime, Time.deltaTime);
        behaviourManager.GetAnim.SetFloat(speedFloat, speed, speedDampTime, Time.deltaTime);
    }

    private IEnumerator PSpeedAudioStart(float duration)
    {
        if (m_PSpeedSource == null) { yield break; } // return , end

        float inititalVolume = m_PSpeedSource.volume;
        float initialPitch = m_PSpeedSource.pitch;

        m_PSpeedSource.Play();

        // Condensed timer, doesn't require outside variables
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            float ratio = t / duration; // 0 to 100%
            m_PSpeedSource.volume = Mathf.Lerp(inititalVolume, 0, curve.Evaluate(ratio));
            m_PSpeedSource.pitch = Mathf.Lerp(initialPitch, 1, curve.Evaluate(ratio));
            yield return null;
        }

        // Assure the final value
        m_PSpeedSource.volume = 1;
        m_PSpeedSource.pitch = 1.5f;


    }

    private IEnumerator PSpeedAudioEnd(float duration)
    {
        if (m_PSpeedSource == null) { yield break; } // return , end


        float finalVolume = m_PSpeedSource.volume;
        float finalPitch = m_PSpeedSource.pitch;

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            float ratio = t / duration;
            //m_PSpeedSource.volume = Mathf.Lerp(finalVolume, 0, ratio); // we're going from 1 to 0
            //m_PSpeedSource.pitch = Mathf.Lerp(finalPitch, 1, ratio); // we're going from 0 to 1
            m_PSpeedSource.volume = Mathf.Lerp(finalVolume, 0, curve.Evaluate(ratio)); // we're going from 1 to 0
            m_PSpeedSource.pitch = Mathf.Lerp(finalPitch, 1, curve.Evaluate(ratio)); // we're going from 0 to 1
            yield return null;
        }

        m_PSpeedSource.Stop();

    }

    // Remove vertical rigidbody velocity.
    private void RemoveVerticalVelocity()
    {
        Vector3 horizontalVelocity = behaviourManager.GetRigidBody.velocity;
        horizontalVelocity.y = 0;
        behaviourManager.GetRigidBody.velocity = horizontalVelocity;
    }

    // Rotate the player to match correct orientation, according to camera and key pressed.
    Vector3 Rotating(float horizontal, float vertical)
    {
        // Get camera forward direction, without vertical component.
        Vector3 forward = behaviourManager.playerCamera.TransformDirection(Vector3.forward);

        // Player is moving on ground, Y component of camera facing is not relevant.
        forward.y = 0.0f;
        forward = forward.normalized;

        // Calculate target direction based on camera forward and direction key.
        Vector3 right = new Vector3(forward.z, 0, -forward.x);
        Vector3 targetDirection = forward * vertical + right * horizontal;

        // Lerp current direction to calculated target direction.
        if ((behaviourManager.IsMoving() && targetDirection != Vector3.zero))
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

            Quaternion newRotation = Quaternion.Slerp(behaviourManager.GetRigidBody.rotation, targetRotation, behaviourManager.turnSmoothing);
            behaviourManager.GetRigidBody.MoveRotation(newRotation);
            behaviourManager.SetLastDirection(targetDirection);
        }
        // If idle, Ignore current camera facing and consider last moving direction.
        if (!(Mathf.Abs(horizontal) > 0.9 || Mathf.Abs(vertical) > 0.9))
        {
            behaviourManager.Repositioning();
        }

        return targetDirection;
    }

    // Collision detection.
    private void OnCollisionStay(Collision collision)
    {
        isColliding = true;
        // Slide on vertical obstacles
        if (behaviourManager.IsCurrentBehaviour(this.GetBehaviourCode()) && collision.GetContact(0).normal.y <= 0.1f)
        {
            GetComponent<CapsuleCollider>().material.dynamicFriction = 0f;
            GetComponent<CapsuleCollider>().material.staticFriction = 0f;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        isColliding = false;
        GetComponent<CapsuleCollider>().material.dynamicFriction = 0.6f;
        GetComponent<CapsuleCollider>().material.staticFriction = 0.6f;
    }

    public void PlayFootStep()
    {
        FootsetpCollection.Audio audio = footsteps.GetFootstep();
        source.pitch = footsteps.PitchVariation();
        source.volume = audio.VolumeVariation();
        //source.volume = 1;
        source.PlayOneShot(audio.clip);
        print("footstep");
    }

    public void PlayLanding()
    {
        //FootsetpCollection.Audio audio = footsteps.GetLanding();
        //source.pitch = footsteps.PitchVariation();
        //source.volume = audio.VolumeVariation();
        //source.PlayOneShot(audio.clip);

    }

}
