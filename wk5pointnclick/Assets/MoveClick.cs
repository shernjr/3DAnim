using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class point_and_click : MonoBehaviour
{
    private NavMeshAgent _agent;
    private Animator _anim;

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo))
            {
                NavMeshHit navHit;

                if (NavMesh.SamplePosition(hitInfo.point, out navHit, 2.0f, NavMesh.AllAreas))
                {
                    _agent.SetDestination(navHit.position);
                    _anim.SetBool("IsWalking", true);
                }
            }
        }

        if (!_agent.pathPending &&
            _agent.remainingDistance <= _agent.stoppingDistance &&
            _agent.velocity.sqrMagnitude == 0f)
        {
            _anim.SetBool("IsWalking", false);
        }
    }
}