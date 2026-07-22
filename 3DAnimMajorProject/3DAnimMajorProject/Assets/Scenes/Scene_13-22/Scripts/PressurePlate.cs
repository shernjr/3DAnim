using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    private Animator anim;
    private bool isPressed = false;
    public PuzzleManager puzzleManager;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void OnTriggerEnter(Collider other)
    {
        // Only trigger once, and only if the Rock hits it
        if (!isPressed && other.CompareTag("Rock"))
        {
            isPressed = true;
            anim.SetTrigger("Depress"); // Play your plate animation
            puzzleManager.PlateActivated(); // Notify the manager
            
            // Optional: Freeze the rock so it stays perfectly on the plate
            //other.attachedRigidbody.isKinematic = true; 
        }
    }
}