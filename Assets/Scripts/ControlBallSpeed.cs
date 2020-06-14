using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlBallSpeed : MonoBehaviour {
    private Rigidbody _rb;
    private float _speed;

    private void Awake() {
        _rb = this.GetComponent<Rigidbody>();        
    }

    public void OnRoundStart() {
        _speed = GameConstants.INITIAL_BALL_SPEED;
    }

    public void OnTurnChange(bool isPlayersTurn) {
        _speed += GameConstants.BALL_SPEED_INCREMENT;
    }

    void FixedUpdate() {
        _rb.velocity = Vector3.Normalize(_rb.velocity) * _speed;            
    }
}
