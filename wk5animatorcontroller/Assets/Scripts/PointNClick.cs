using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PointNClick : MonoBehaviour
{
    private NavMeshAgent agent;
    public Animator anim;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Mouse.current.leftButton.IsPressed()) 
        {
            Vector2 mouseScreenPosition = Mouse.current.position.ReadValue();
            Ray canRayCast = Camera.main.ScreenPointToRay(mouseScreenPosition);

            if (Physics.Raycast(canRayCast, out RaycastHit hitInfo))
            {
                agent.SetDestination(hitInfo.point);
            }
        }

        if (animator != null)
        {
            // Using remainingDistance is usually smoother than hasPath
            bool isMoving = agent.remainingDistance > agent.stoppingDistance;
            
            anim.SetBool("isMoving", isMoving);
            
            // 4. Fixed "setFloat" to "SetFloat" (Capital S)
            // This calculates a 0 to 1 value based on current speed
            anim.SetFloat("moveNot", agent.velocity.magnitude / agent.speed);
        }
    }
}
