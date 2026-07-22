using UnityEngine;
using System.Collections;
using StarterAssets;

[RequireComponent(typeof(Animator))]
public class PlayerInteractionIK : MonoBehaviour
{
    private Animator anim;
    private StarterAssetsInputs _input;
    private ThirdPersonController movement;
    private CharacterController controller;

    // Runtime state tracking
    private bool isNearRock = false;
    private bool isNearTorch = false;
    private bool isExploded = false;
    
    private Transform currentTarget;
    private float torchIKWeight = 0f;

    [Header("Rock Puzzle Settings")]
    [Range(0, 1)] public float rockHandWeight = 1f;
    public float rockDetectDistance = 0.8f;

    [Header("Torch Trap Settings")]
    public float torchReachSpeed = 4f;
    public float torchExplodeDistance = 0.6f;

    [Header("Layer Filtering")]
    public LayerMask interactionLayer; // Assign both 'Pushable' and 'Default' in the inspector

    void Start()
    {
        anim = GetComponent<Animator>();
        _input = GetComponentInParent<StarterAssetsInputs>();
        movement = GetComponentInParent<ThirdPersonController>();
        controller = GetComponentInParent<CharacterController>();
    }

    void Update()
    {
        if (isExploded) return;

        RaycastHit hit;
        Vector3 rayStart = transform.position + (Vector3.up * 1.0f); // Chest height
        
        // We use a SphereCast (0.25m thickness) instead of a thin raycast 
        // so it reliably clips the torch box on the wall as you walk past!
        float sweepRadius = 0.25f; 
        Debug.DrawRay(rayStart, transform.forward * 1.5f, Color.green);

        if (Physics.SphereCast(rayStart, sweepRadius, transform.forward, out hit, 1.5f, interactionLayer))
        {
            if (hit.collider.CompareTag("Rock"))
            {
                ClearTorchState();
                isNearRock = true;
                currentTarget = hit.collider.transform;

                if (_input != null && _input.move.y > 0.1f)
                {
                    anim.SetBool("IsPushing", true);
                }
                else
                {
                    anim.SetBool("IsPushing", false);
                }
            }
            else if (hit.collider.CompareTag("Torch"))
            {
                ClearRockState();
                isNearTorch = true;
                currentTarget = hit.collider.transform;

                // Smoothly extend the arm outward toward the target
                torchIKWeight = Mathf.MoveTowards(torchIKWeight, 1f, Time.deltaTime * torchReachSpeed);

                // Blow up if the distance to the surface reaches your threshold
                if (hit.distance <= torchExplodeDistance)
                {
                    InteractWithTorch();
                }
            }
            else
            {
                ResetAllStates();
            }
        }
        else
        {
            ResetAllStates();
        }
    }

    private void ClearRockState()
    {
        isNearRock = false;
        anim.SetBool("IsPushing", false);
    }

    private void ClearTorchState()
    {
        isNearTorch = false;
        torchIKWeight = 0f;
    }

    private void ResetAllStates()
    {
        ClearRockState();
        ClearTorchState();
        currentTarget = null;
    }

    void OnAnimatorIK(int layerIndex)
    {
        if (!anim) return;

        // 1. ROCK PUZZLE PASS
        if (isNearRock && currentTarget != null && anim.GetBool("IsPushing"))
        {
            Transform leftHand = currentTarget.Find("IK_LeftHand");
            Transform rightHand = currentTarget.Find("IK_RightHand");

            if (leftHand && rightHand)
            {
                ApplyHandIK(AvatarIKGoal.LeftHand, leftHand, rockHandWeight);
                ApplyHandIK(AvatarIKGoal.RightHand, rightHand, rockHandWeight);
            }
            return; // Exit early so clean up blocks don't execute
        }

        // 2. TORCH TRAP PASS
        if (isNearTorch && currentTarget != null && torchIKWeight > 0f)
        {
            Transform torchTarget = currentTarget.Find("IK_Target");
            if (torchTarget != null)
            {
                ApplyHandIK(AvatarIKGoal.RightHand, torchTarget, torchIKWeight);
                
                // Keep the left hand uninhibited by manual weights
                anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0f);
                anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0f);
            }
            return;
        }

        // 3. FALLBACK SAFETY: Clear everything cleanly if no targets exist
        anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0f);
        anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0f);
        anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 0f);
        anim.SetIKRotationWeight(AvatarIKGoal.RightHand, 0f);
    }

    private void ApplyHandIK(AvatarIKGoal goal, Transform target, float weight)
    {
        anim.SetIKPositionWeight(goal, weight);
        anim.SetIKRotationWeight(goal, weight);
        anim.SetIKPosition(goal, target.position);
        anim.SetIKRotation(goal, target.rotation);
    }


    private void InteractWithTorch()
    {
        Transform activeTorch = currentTarget;
        ResetAllStates(); // Drops the hand weight back to 0 so you can walk away

        // SCENARIO A: It is an explosive trap!
        if (activeTorch.TryGetComponent(out TorchObject trapTorch))
        {
            isExploded = true; // Locks out further interactions
            trapTorch.Explode();
            
            anim.SetTrigger("GetHit");

            // Calculate blast pushback
            Vector3 blastSourcePosition = activeTorch.position;
            Vector3 playerFloorPos = transform.position;
            Vector3 pushDirection = (playerFloorPos - blastSourcePosition);
            pushDirection.y = 0f; 
            pushDirection.Normalize();

            if (controller != null)
            {
                StartCoroutine(ExecuteSafeKnockback(pushDirection));
            }
        }
        // SCENARIO B: It is a harmless puzzle torch!
        else if (activeTorch.TryGetComponent(out FalseWallTorchPuzzle puzzleTorch))
        {
            // Light the torch and tell the manager to count it
            puzzleTorch.Interact();
            
            // Notice we DO NOT set 'isExploded = true' and we DO NOT trigger knockbacks here!
            // This allows the player to safely walk to the next puzzle torch.
        }
    }

    private IEnumerator ExecuteSafeKnockback(Vector3 direction)
    {
        // Fetch reference components safely
        ThirdPersonController tpc = GetComponent<ThirdPersonController>();
        StarterAssetsInputs inputs = GetComponent<StarterAssetsInputs>();

        // Step A: Stop user input actions from fighting our code positions
        if (inputs != null) inputs.move = Vector2.zero;
        if (tpc != null) tpc.enabled = false; // Suspends internal locomotion updates cleanly

        // Step B: Set up displacement curves
        float startingSpeed = 32f;  // Initial burst velocity away from the blast zone
        float totalDuration = 0.3f; // Time frame window for the push translation
        float elapsedTime = 0f;

    while (elapsedTime < totalDuration)
        {
            // Calculate a non-linear decay curve (t normalized)
            float t = elapsedTime / totalDuration;
            
            // Using (1 - t)^2 creates an aggressive high-speed burst that drops off fast
            float speedMultiplier = (1f - t) * (1f - t);
            float currentSpeed = startingSpeed * speedMultiplier;

            Vector3 frameVelocity = direction * currentSpeed;

            // Keep character firmly pinned to the deck geometry
            frameVelocity.y = -6.0f; 

            // Apply physical movement directly to the controller architecture
            controller.Move(frameVelocity * Time.deltaTime);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Step C: Restore controller logic once recovery frame finishes processing
        if (tpc != null) tpc.enabled = true;
        isExploded = false; // Resets the trap state so the player can interact with other torches if needed
    }
}