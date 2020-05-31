using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleController : MonoBehaviour { 
    public bool leftHand;
    
    private GameObject controllerObj;
    private Rigidbody rigidbody;

    private void Start() {
        rigidbody = this.GetComponent<Rigidbody>();        
    }

    private void OnEnable() {
        controllerObj = leftHand ? GameObject.FindGameObjectWithTag("LeftHand") :
                                    GameObject.FindGameObjectWithTag("RightHand");
    }

    void FixedUpdate() {
        rigidbody.MovePosition(controllerObj.transform.position);
        rigidbody.MoveRotation(controllerObj.transform.rotation);
        rigidbody.velocity = Vector3.zero;
    }
}
