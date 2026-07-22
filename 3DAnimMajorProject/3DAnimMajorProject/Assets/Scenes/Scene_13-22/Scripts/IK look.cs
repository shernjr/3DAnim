using UnityEngine;

public class IKlook : MonoBehaviour
{
    Animator animator;
    public GameObject target;
    public float weight = 0.0f;
    private bool start = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    private void OnAnimatorIK(int layerIndex)
    {
        animator.SetLookAtPosition(target.transform.position);
        animator.SetLookAtWeight(weight);
    }
    // Update is called once per frame
    void Update()
    {
        if (start) {
            if (!(weight >= 1f))
            {
                weight += 0.01f;
            }
        }
    }
    public void LookOn()
    {
        start = true;
    }
    public void LookOff()
    {
        start = false;
        weight = 0.0f;
    }
}
