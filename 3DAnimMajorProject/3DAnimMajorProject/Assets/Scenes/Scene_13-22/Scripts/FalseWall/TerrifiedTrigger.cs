using UnityEngine;

public class TerrifiedTrigger : MonoBehaviour
{
    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player stepped into the zone
        if (!hasTriggered && other.CompareTag("Player"))
        {
            hasTriggered = true;
            
            Animator anim = other.GetComponent<Animator>();
            if (anim != null)
            {
                anim.SetTrigger("Terrified");
            }
        }
    }
}