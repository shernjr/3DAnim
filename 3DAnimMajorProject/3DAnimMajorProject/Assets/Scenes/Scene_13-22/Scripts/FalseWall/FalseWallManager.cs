using UnityEngine;

public class FalseWallManager : MonoBehaviour
{
    [Header("Puzzle Settings")]
    public Animator falseWallAnimator;
    public int requiredTorches = 3;

    public ParticleSystem sparks;
    
    private int litTorchesCount = 0;

    // The torches will call this method when interacted with
    public void TorchLit()
    {
        litTorchesCount++;

        // Check if all torches are lit
        if (litTorchesCount >= requiredTorches)
        {
            SolvePuzzle();
        }
    }

    private void SolvePuzzle()
    {
        if (falseWallAnimator != null)
        {
            // Trigger whatever parameter you use to open the wall.
            // (e.g., "Open", "Solve", etc.)
            falseWallAnimator.SetTrigger("Open"); 
            sparks.Play();
        }
    }
}