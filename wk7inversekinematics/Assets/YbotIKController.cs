using UnityEngine;
using UnityEngine.InputSystem;
public class YbotIKController : MonoBehaviour
{
	
	public Animator animator;
    public Transform targetTransform;
    private Vector3 targetPosition;
    public float IKWeight;
    public Transform handHardPoint;
    public Transform RightElbowHintTransform;
    private Vector3 RightElbowHintPosition;

    public bool isUpdatingIKPositions;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (InputSystem.actions.FindAction("Interact").WasPerformedThisFrame()) 
        {
            animator.SetTrigger("pickup");
            isUpdatingIKPositions = true;
        }

        if (InputSystem.actions.FindAction("Throw").WasPerformedThisFrame())
        {
            animator.SetTrigger("throw");
        }


        
    }

    private void OnAnimatorIK(int LayerIndex)
    {
        if (isUpdatingIKPositions)
        {
            targetPosition = targetTransform.position;
            RightElbowHintPosition = RightElbowHintTransform.position;
        }
       

        IKWeight = animator.GetFloat("IKValue");

        animator.SetIKPosition(AvatarIKGoal.RightHand, targetPosition);
        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, IKWeight);

        animator.SetIKHintPosition(AvatarIKHint.RightElbow, RightElbowHintPosition);
        animator.SetIKHintPositionWeight(AvatarIKHint.RightElbow, IKWeight);

        animator.SetLookAtPosition(targetPosition);
        animator.SetLookAtWeight(IKWeight);
    }

    private void OnPickup()
    {
        isUpdatingIKPositions = false;
        targetTransform.SetParent(handHardPoint);
        targetTransform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        Rigidbody rb = targetTransform.GetComponent<Rigidbody>();
        
        if (rb != null)
        {
            rb.isKinematic = true; // This stops gravity/physics from pulling it out of your hand
        }

    }

    private void OnThrow()
    {
        //Throw animation event Throw calls this method.
        //At the end of the throw animation, we want to unparent the object and apply a force to it.
        if (targetTransform == null) return;
        targetTransform.SetParent(null);
        Rigidbody rb = targetTransform.GetComponent<Rigidbody>();
        if (rb != null)        
        {
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.AddForce(transform.forward * 10f, ForceMode.Impulse);
        }
        
    }
}


