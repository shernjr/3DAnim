using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OpeningDoor : MonoBehaviour
{
    Animator anim;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = this.transform.parent.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) 
    {
        anim.SetBool("isOpen", true);
    }

    private void OnTriggerExit(Collider other) 
    {
        anim.SetBool("isOpen", false);
    }
}
