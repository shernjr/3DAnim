using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using static UnityEngine.GraphicsBuffer;

public class point : MonoBehaviour
{
    Animator animator;
    public GameObject target;
    public GameObject target2;
    public GameObject r_hand;
    public GameObject l_hand;
    public float IK_weight = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    private void OnAnimatorIK(int layerIndex)
    {
        IK_weight = animator.GetFloat("IK_grab");
        if (IK_weight > 0.9f)
        {
            target.transform.parent = l_hand.transform;
            target.transform.localPosition = new Vector3(-0.2f,0.03f,-0.18f);
            target.transform.localRotation = Quaternion.Euler(-140.84f,-52.8f,72.5f);
        }
        animator.SetIKPosition(AvatarIKGoal.LeftHand, target.transform.position);
        animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, IK_weight);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void Grab()
    {
            animator.SetTrigger("Grab");
    }
    public void Drop()
    {
        target.transform.parent = null;
        target.transform.localPosition = new Vector3(28.00474f,3.2f,-6.3f);
        target.transform.localRotation = Quaternion.Euler(-94.3f, -49.28f, 164.348f);
    }
    public void GrabRope()
    {
        target2.transform.parent = r_hand.transform;
        target2.transform.localPosition = new Vector3(1.34f, -0.05f, 0.23f); //(-0.08f, -0.05f, -0.25f), (0.43f, -0.05f, 0.08f)
        //target2.transform.localRotation = Quaternion.Euler(-140.84f, -52.8f, 72.5f);
        animator.SetIKPosition(AvatarIKGoal.RightHand, target2.transform.position);

    }
    public void DropRope()
    {
        target2.transform.parent = null;
    }
}
