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
    private Rigidbody rigidbody;

    private void Start() {
        rigidbody = this.GetComponent<Rigidbody>();        
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
        rigidbody.MovePosition(controllerObj.transform.position);
        rigidbody.MoveRotation(controllerObj.transform.rotation);
        rigidbody.velocity = Vector3.zero;
    }
}
