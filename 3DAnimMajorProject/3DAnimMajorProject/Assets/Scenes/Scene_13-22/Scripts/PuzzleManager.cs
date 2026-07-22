using UnityEngine;
using UnityEngine.Playables;

public class PuzzleManager : MonoBehaviour
{
    public int platesActivated = 0;
    public int requiredPlates = 2;
    public Animator gateAnimator;
    public GameObject timelineTriggerZone; // The box collider past the gate

    void Start()
    {
        timelineTriggerZone.SetActive(false); // Disable timeline trigger initially
    }

    public void PlateActivated()
    {
        platesActivated++;
        if (platesActivated >= requiredPlates)
        {
            OpenGate();
        }
    }

    void OpenGate()
    {
        gateAnimator.SetTrigger("Open");
        timelineTriggerZone.SetActive(true); // Enable the zone to trigger the timeline
    }
}