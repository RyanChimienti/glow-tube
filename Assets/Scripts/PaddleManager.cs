using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class PaddleManager : MonoBehaviour {
    private GameObject _handObj;
    private Rigidbody _rb;

    private void Start() {
        _rb = this.GetComponent<Rigidbody>();        
    }

    private void OnEnable() {
        bool paddleInLeftHand = System.Convert.ToBoolean(PlayerPrefs.GetInt("PaddleInLeftHand"));
        _handObj = paddleInLeftHand ? GameObject.FindGameObjectWithTag("LeftHand") :
                                    GameObject.FindGameObjectWithTag("RightHand");
    }

    private void FixedUpdate() {
        // We move the paddle the same way grabbed objects follow the controller
        // in Unity XR Interaction Toolkit.
        _rb.MovePosition(_handObj.transform.position);
        _rb.MoveRotation(_handObj.transform.rotation);
        _rb.velocity = Vector3.zero;
    }
}
