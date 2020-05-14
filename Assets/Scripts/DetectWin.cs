using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectWin : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<MeshRenderer>().enabled = false;
    }

    void OnTriggerEnter(Collider otherCollider)
    {
        if (otherCollider.gameObject.tag == "Ball")
        {
            GameObject.FindWithTag("Ball")
                .GetComponent<Rigidbody>().velocity = Vector3.zero;
            this.GetComponent<MeshRenderer>().enabled = true;
        }    
    }
}
