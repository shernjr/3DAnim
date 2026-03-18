using UnityEngine;

public class BridgeSensorScript : MonoBehaviour
{


   [SerializeField] private Animator BridgeAnim;
   [SerializeField] private string playerTag = "Player";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag(playerTag))
        {
            BridgeAnim.SetBool("isTriggered", true);
            BridgeAnim.SetFloat("moveNot", 1f);
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        if(other.CompareTag(playerTag))
        {
            BridgeAnim.SetBool("isTriggered", false);
            BridgeAnim.SetFloat("moveNot", -1.0f);
        }
    }

}
