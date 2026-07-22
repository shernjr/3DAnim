using UnityEngine;

public class TorchObject : MonoBehaviour
{
    [Header("References")]
    public ParticleSystem explosionFX; 
    public Animator doorAnimator;
    public GameObject torchVisualMesh;

    public void Explode()
    {
        // 1. FIX: Instantly unparent the explosion object from EVERYTHING 
        // to protect it from hierarchy deactivation loops
        if (explosionFX != null)
        {
            explosionFX.transform.SetParent(null, true); // Keep world position intact
            explosionFX.gameObject.SetActive(true);      // Ensure it's active
            explosionFX.Play();
            
            // Clean up the particle GameObject from the scene hierarchy after 3 seconds
            Destroy(explosionFX.gameObject, 3f); 
        }

        // 2. Safely clear the visual mesh now that the particles are fully detached
        if (torchVisualMesh != null) 
        {
            torchVisualMesh.SetActive(false);
        }

        // 3. Trigger the door frame animator sequence
        if (doorAnimator != null) 
        {
            doorAnimator.SetBool("Open", true);
        }
        
        // 4. Safely kill the interaction trigger zone collider
        if (TryGetComponent(out Collider col))
        {
            col.enabled = false;
        }
    }
}