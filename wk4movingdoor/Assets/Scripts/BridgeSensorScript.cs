using UnityEngine;

// This line ensures the object has an AudioSource so you don't get null errors
[RequireComponent(typeof(AudioSource))] 
public class BridgeSensorScript : MonoBehaviour
{
    [SerializeField] private Animator BridgeAnim;
    [SerializeField] private string playerTag = "Player";
    [SerializeField] public AudioClip bridgeSound;   
    
    private AudioSource audioSource; // Reference to the component

    void Start()
    {
        // Link the variable to the component on this object
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag(playerTag))
        {
            BridgeAnim.SetBool("isTriggered", true);
            BridgeAnim.SetFloat("moveNot", 1f);

            // Use PlayOneShot so it doesn't get interrupted if triggered rapidly
            if (bridgeSound != null)
            {
                audioSource.PlayOneShot(bridgeSound);
            }
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        if(other.CompareTag(playerTag))
        {
            BridgeAnim.SetBool("isTriggered", false);
            BridgeAnim.SetFloat("moveNot", -1.0f);
            
            // Optional: Play the sound again (or a different one) when closing
            // if (bridgeSound != null)
            // {
            //     audioSource.PlayOneShot(bridgeSound);
            // }
        }
    }
}