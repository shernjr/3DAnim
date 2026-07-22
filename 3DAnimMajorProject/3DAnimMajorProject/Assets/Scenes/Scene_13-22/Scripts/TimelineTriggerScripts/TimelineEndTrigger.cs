using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.Playables;
using System.Collections.Generic; // Required to use Lists!
using StarterAssets; 
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem; 
#endif

public class TimelineEndTrigger : MonoBehaviour
{
    [Header("References")]
    public PlayableDirector director;
    public GameObject player; 

    [Header("Script Toggles (When Timeline Ends)")]
    [Tooltip("Drag scripts here that you want to turn ON (e.g., IK scripts).")]
    public List<MonoBehaviour> scriptsToEnable;
    
    [Tooltip("Drag scripts here that you want to turn OFF.")]
    public List<MonoBehaviour> scriptsToDisable;

    [Header("Cinemachine Toggles")]
    [Tooltip("Drag cinemachine cameras here that you want to turn ON.")]
    public List<CinemachineCamera> cinemachineCamerasToEnable;

    [Tooltip("Drag cinemachine cameras here that you want to turn OFF.")]
    public List<CinemachineCamera> cinemachineCamerasToDisable;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 1. Stop the timeline immediately
            if (director != null)
            {
                director.Stop();
            }

            // 2. Toggle the custom lists of scripts
            ToggleScriptsAndCameras();

            // 3. Return interaction control to the player
            GiveControlBack(other.gameObject);

            // 4. Disable this trigger so it doesn't execute again
            gameObject.SetActive(false);
        }
    }

    private void ToggleScriptsAndCameras()
    {
        // Toggle scripts
        foreach (MonoBehaviour script in scriptsToEnable)
        {
            if (script != null) script.enabled = true;
        }

        foreach (MonoBehaviour script in scriptsToDisable)
        {
            if (script != null) script.enabled = false;
        }

        // Turn ON the requested cinemachine camera GameObjects
        foreach (CinemachineCamera camera in cinemachineCamerasToEnable)
        {
            if (camera != null) camera.gameObject.SetActive(true); 
        }

        // Turn OFF the requested cinemachine camera GameObjects
        foreach (CinemachineCamera camera in cinemachineCamerasToDisable)
        {
            if (camera != null) camera.gameObject.SetActive(false);
        }
    }

    private void GiveControlBack(GameObject player)
    {
        ThirdPersonController controller = player.GetComponent<ThirdPersonController>();
        if (controller != null)
        {
            controller.enabled = true;
        }

#if ENABLE_INPUT_SYSTEM
        PlayerInput playerInput = player.GetComponent<PlayerInput>();
        if (playerInput != null)
        {
            playerInput.enabled = true;
        }
#endif

        StarterAssetsInputs inputs = player.GetComponent<StarterAssetsInputs>();
        if (inputs != null)
        {
            inputs.move = Vector2.zero;
            inputs.jump = false;
            inputs.sprint = false;
        }
    }
}