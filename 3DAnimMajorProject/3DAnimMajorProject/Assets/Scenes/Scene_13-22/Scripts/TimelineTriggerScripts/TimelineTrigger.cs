using UnityEngine;
using UnityEngine.Playables;
using Unity.Cinemachine; // Updated for Unity 6!
using System.Collections.Generic;

public class TimelineTrigger : MonoBehaviour
{
    [Header("References")]
    public PlayableDirector director;
    public GameObject player; 

    [Header("Script Toggles (When Timeline Starts)")]
    [Tooltip("Drag scripts here that you want to turn ON.")]
    public List<MonoBehaviour> scriptsToEnable;
    
    [Tooltip("Drag scripts here that you want to turn OFF (e.g., IK scripts).")]
    public List<MonoBehaviour> scriptsToDisable;

    [Header("Cinemachine Toggles")]
    [Tooltip("Drag cinemachine cameras here that you want to turn ON.")]
    public List<CinemachineCamera> cinemachineCamerasToEnable; // Updated for Unity 6!

    [Tooltip("Drag cinemachine cameras here that you want to turn OFF.")]
    public List<CinemachineCamera> cinemachineCamerasToDisable; // Updated for Unity 6!

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (director != null)
            {
                director.Play();
            }

            ToggleScriptsAndCameras();
            gameObject.SetActive(false); 
        }
    }

    private void ToggleScriptsAndCameras()
    {
        // Turn on the requested scripts
        foreach (MonoBehaviour script in scriptsToEnable)
        {
            if (script != null) script.enabled = true;
        }

        // Turn off the requested scripts
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
}