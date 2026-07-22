using UnityEngine;
using System.Collections;
using StarterAssets;

[RequireComponent(typeof(Animator))]
public class PlayerTorchTrapIK : MonoBehaviour
{
    private Animator anim;
    private ThirdPersonController movement;
    private CharacterController controller;
    
    private bool isNearTorch = false;
    private Transform currentTorch;
    private float currentWeight = 0f;
    private bool isExploded = false;

    [Header("IK Configuration")]
    public float reachSpeed = 4f;       // How fast the hand reaches out
    public float explodeDistance = 0.55f; // Distance where the hand touches and it blows up

    void Start()
    {
        anim = GetComponent<Animator>();
        
        // Since this script lives on the child mesh, find components on the parent root
        movement = GetComponentInParent<ThirdPersonController>();
        controller = GetComponentInParent<CharacterController>();
    }

    void Update()
    {
        if (isExploded) return;

        RaycastHit hit;
        // Using your exact proven raycast origin height!
        Vector3 rayStart = transform.position + (Vector3.up * 0.5f);
        
        // Draw a blue line in scene view to trace your torch detection
        Debug.DrawRay(rayStart, transform.forward * 1.5f, Color.blue);

        if (Physics.Raycast(rayStart, transform.forward, out hit, 1.5f))
        {
            if (hit.collider.CompareTag("Torch"))
            {
                isNearTorch = true;
                currentTorch = hit.collider.transform;

                // Smoothly blend the hand weight up as you approach
                currentWeight = Mathf.MoveTowards(currentWeight, 1f, Time.deltaTime * reachSpeed);

                // When the laser distance drops to your touch limit, trigger detonation!
                if (hit.distance <= explodeDistance)
                {
                    TriggerTorchExplosion();
                }
            }
            else
            {
                ResetReaching();
            }
        }
        else
        {
            ResetReaching();
        }
    }

    private void ResetReaching()
    {
        isNearTorch = false;
        currentTorch = null;
        currentWeight = Mathf.MoveTowards(currentWeight, 0f, Time.deltaTime * reachSpeed);
    }

    void OnAnimatorIK(int layerIndex)
    {
        // Reach out if the raycast sees the torch and weight is active
        if (anim && isNearTorch && currentTorch != null && currentWeight > 0f)
        {
            Transform handTarget = currentTorch.Find("IK_Target");

            if (handTarget != null)
            {
                anim.SetIKPositionWeight(AvatarIKGoal.RightHand, currentWeight);
                anim.SetIKRotationWeight(AvatarIKGoal.RightHand, currentWeight);
                anim.SetIKPosition(AvatarIKGoal.RightHand, handTarget.position);
                anim.SetIKRotation(AvatarIKGoal.RightHand, handTarget.rotation);
            }
        }
        else if (anim)
        {
            // Fully return control back to standard animations
            anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 0f);
            anim.SetIKRotationWeight(AvatarIKGoal.RightHand, 0f);
        }
    }

    private void TriggerTorchExplosion()
    {
        isExploded = true;
        ResetReaching();

        // Find the companion script on the specific torch asset we hit
        if (currentTorch.TryGetComponent(out TorchObject torch))
        {
            torch.Explode();
        }

        // Fire your hit animator trigger state
        anim.SetTrigger("GetHit");

        // Calculate horizontal push vector directly backwards from where you are facing
        Vector3 pushDirection = -transform.forward; 
        pushDirection.y = 0f;
        pushDirection.Normalize();

        if (controller != null && movement != null)
        {
            StartCoroutine(ApplyKnockbackVelocity(pushDirection));
        }
    }

    private IEnumerator ApplyKnockbackVelocity(Vector3 direction)
    {
        movement.enabled = false;

        float knockbackForce = 13f;
        float knockbackDuration = 0.35f;
        float timer = 0f;

        while (timer < knockbackDuration)
        {
            float currentForce = Mathf.Lerp(knockbackForce, 0f, timer / knockbackDuration);
            Vector3 finalMoveVector = direction * currentForce;
            
            // Keep player grounded during force application
            finalMoveVector.y -= 9.81f; 

            controller.Move(finalMoveVector * Time.deltaTime);

            timer += Time.deltaTime;
            yield return null;
        }

        movement.enabled = true;
    }
}