using UnityEngine;

public class FalseWallTorchPuzzle : MonoBehaviour
{
    [Header("References")]
    public FalseWallManager manager;
    public ParticleSystem fireFX; 
    
    private bool isLit = false;

    // Call this from your PlayerReachIK script instead of Explode()
    public void Interact() 
    {
        if (isLit) return; // Prevent double counting!

        isLit = true;

        // 1. Turn on the flame
        if (fireFX != null)
        {
            fireFX.gameObject.SetActive(true);
            fireFX.Play();
        }

        // 2. Tell the manager we lit a torch
        if (manager != null)
        {
            manager.TorchLit();
        }

        // 3. Disable the interaction collider so the player can't interact again
        if (TryGetComponent(out Collider col))
        {
            col.enabled = false;
        }
    }
}