using UnityEngine;

public class Ball_Bell : MonoBehaviour
{
    private AudioSource audioSource;
    // Drag your bell sound clip here in the inspector
    public AudioClip bellSound; 

    void Start()
    {
        // Get the AudioSource component attached to the ball
        audioSource = GetComponent<AudioSource>();
    }

    void OnCollisionEnter(Collision collision)
    {
        // Play the sound once on collision
        if (bellSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(bellSound);
        }
    }
}