using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class KidWASDController : MonoBehaviour
{

    public Animator anim;
    public float moveSpeed = 5f;
    public float turnSpeed = 90;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 moveValue = InputSystem.actions.FindAction("Move").ReadValue<Vector2>();

        float forwardValue = moveValue.y;
        float rightValue = moveValue.x;

        transform.Translate(0, 0, forwardValue * Time.deltaTime * moveSpeed, Space.Self);
        transform.Rotate(0, rightValue * Time.deltaTime * turnSpeed, 0);

        anim.SetBool("isMoving", forwardValue != 0);
        anim.SetFloat("moveNot", forwardValue);
    }
}
