using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class PaddleController : MonoBehaviour {
    [Tooltip("The object containing the GameController script")]
    public GameObject gameControllerObj;

    /// <summary>
    /// True if the paddle is held in the left hand; false otherwise.
    /// </summary>
    public bool LeftHand { get; set; }

    private GameObject controllerObj;
    private Rigidbody rb;

    private void Start() {
        rb = this.GetComponent<Rigidbody>();        
    }

    private void OnEnable() {
        controllerObj = LeftHand ? GameObject.FindGameObjectWithTag("LeftHand") :
                                    GameObject.FindGameObjectWithTag("RightHand");
    }

    public void OnCollisionEnter(Collision collision) {
        if (collision.collider.tag == "Ball") {
            gameControllerObj.GetComponent<GameController>().HandleBallHit(true);
        }
    }

    void FixedUpdate() {
        // We move the paddle the same way grabbed objects follow the controller
        // in Unity XR Interaction Toolkit.
        rb.MovePosition(controllerObj.transform.position);
        rb.MoveRotation(controllerObj.transform.rotation);
        rb.velocity = Vector3.zero;
    }
}
