using UnityEngine;

public class IK : MonoBehaviour
{
    Animator animator;
    public GameObject target;
    public float IK_weight = 1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnAnimatorIK(int layerIndex)
    {
        animator.SetIKPosition(AvatarIKGoal.RightHand, target.transform.position);
        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, IK_weight);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
