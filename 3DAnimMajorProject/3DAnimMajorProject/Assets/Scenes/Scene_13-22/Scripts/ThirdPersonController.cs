using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ThirdPersonController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float turnSpeed = 15f;
    public Transform cameraTransform;

    [Header("Physics Interaction")]
    public float pushForce = 10f; // How hard the player pushes the rock

    public bool isControlEnabled = true;

    private Rigidbody rb;
    private Animator anim;
    private Vector3 moveInput;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        // Automatically grab the main camera if not assigned
        if (cameraTransform == null && Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    void Update()
    {

        if (!isControlEnabled) 
        {
            anim.SetFloat("Speed", 0f); // Force idle animation during cutscenes
            return; 
        }

        // Get raw WASD / Joystick input
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // Calculate movement direction relative to the camera's orientation
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;
        
        forward.y = 0f; // Keep movement strictly on the flat ground plane
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        moveInput = (forward * vertical + right * horizontal).normalized;

        // Feed movement intensity to your Animator (Make sure your Animator has a float parameter named "Speed")
        float currentSpeed = moveInput.magnitude;
        anim.SetFloat("Speed", currentSpeed);
    }

    void FixedUpdate()
    {
        if (moveInput.magnitude > 0.1f)
        {
            // Move the Rigidbody physically
            Vector3 targetVelocity = moveInput * moveSpeed;
            rb.MovePosition(rb.position + targetVelocity * Time.fixedDeltaTime);

            // Smoothly rotate the character to face the direction they are walking
            Quaternion targetRotation = Quaternion.LookRotation(moveInput);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, turnSpeed * Time.fixedDeltaTime));
        }
    }

    // Custom physics handling for pushing the rock smoothly
    void OnCollisionStay(Collision collision)
    {
        if (collision.collider.CompareTag("Rock"))
        {
            Rigidbody rockRb = collision.collider.GetComponent<Rigidbody>();
            if (rockRb != null && !rockRb.isKinematic)
            {
                // Push direction travels outward from the player to the rock
                Vector3 pushDirection = collision.transform.position - transform.position;
                pushDirection.y = 0; // Don't lift or push the rock into the dirt

                rockRb.AddForce(pushDirection.normalized * pushForce, ForceMode.Force);
            }
        }
    }
}