using UnityEngine;
using StarterAssets; 

[RequireComponent(typeof(Animator))]
public class PlayerPushIK : MonoBehaviour
{
    private Animator anim;
    private StarterAssetsInputs _input; 
    private bool isNearRock = false;
    private Transform currentRock;
    
    [Header("IK Weights")]
    [Range(0, 1)] public float handWeight = 1f;

    [Header("Detection Settings")]
    public float detectDistance = 0.8f; // Shortened so you physically bump the rock!
    public LayerMask pushableLayer; // We will assign this in the Inspector

    void Start()
    {
        anim = GetComponent<Animator>();
        _input = GetComponent<StarterAssetsInputs>();
    }

    void Update()
    {
        RaycastHit hit;
        // Start the raycast from chest height (1 meter up)
        Vector3 rayStart = transform.position + (Vector3.up * 1.0f); 
        
        // Draws a red laser in the Scene view so you can visually verify the hit
        Debug.DrawRay(rayStart, transform.forward * detectDistance, Color.red);

        // Uses a LayerMask so the raycast ignores the player's own colliders
        if (Physics.Raycast(transform.position + (Vector3.up * 0.5f), transform.forward, out hit, 1.5f))
        {
            if (hit.collider.CompareTag("Rock"))
            {
                isNearRock = true;
                currentRock = hit.collider.transform;
                
                // If pressing forward ('W' key or joystick up)
                if (_input != null && _input.move.y > 0.1f) 
                {
                    anim.SetBool("IsPushing", true);
                }
                else
                {
                    anim.SetBool("IsPushing", false);
                }
            }
        }
        else
        {
            ResetPushing();
        }
    }

    private void ResetPushing()
    {
        isNearRock = false;
        currentRock = null;
        anim.SetBool("IsPushing", false);
    }

    void OnAnimatorIK(int layerIndex)
    {
        if (anim && isNearRock && currentRock != null && anim.GetBool("IsPushing"))
        {
            Transform leftHandTarget = currentRock.Find("IK_LeftHand");
            Transform rightHandTarget = currentRock.Find("IK_RightHand");

            if (leftHandTarget && rightHandTarget)
            {
                anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, handWeight);
                anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, handWeight);
                anim.SetIKPosition(AvatarIKGoal.LeftHand, leftHandTarget.position);
                anim.SetIKRotation(AvatarIKGoal.LeftHand, leftHandTarget.rotation);

                anim.SetIKPositionWeight(AvatarIKGoal.RightHand, handWeight);
                anim.SetIKRotationWeight(AvatarIKGoal.RightHand, handWeight);
                anim.SetIKPosition(AvatarIKGoal.RightHand, rightHandTarget.position);
                anim.SetIKRotation(AvatarIKGoal.RightHand, rightHandTarget.rotation);
            }
        }
        else
        {
            anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0);
            anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0);
            anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
            anim.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);
        }
    }
}