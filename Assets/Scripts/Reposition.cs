using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reposition : MonoBehaviour {
    private bool _repositionAllowed;
    private bool _repositioningLeft;
    private bool _repositioningRight;

    private void Awake() {
        _repositionAllowed = true;
        _repositioningLeft = false;
        _repositioningRight = false;
    }

    public void OnHandsChange(bool controllersActive) {
        // Reposition is only allowed when the controllers are held,
        // since the reposition buttons are on the controllers.
        _repositionAllowed = controllersActive;
        
        if (!controllersActive) {
            _repositioningLeft = false;
            _repositioningRight = false;
        }
    }

    public void OnLeftSecondaryButtonDown() {
        if (_repositionAllowed) {
            _repositioningLeft = true;
        }
    }

    public void OnLeftSecondaryButtonUp() {
        _repositioningLeft = false;
    }

    public void OnRightSecondaryButtonDown() {
        if (_repositionAllowed) {
            _repositioningRight = true;
        }
    }

    public void OnRightSecondaryButtonUp() {
        _repositioningRight = false;
    }

    public void FixedUpdate() {
        if (!_repositionAllowed) {
            return;
        }

        if (_repositioningRight) {
            MoveArena(false);
        }
        else if (_repositioningLeft) {
            MoveArena(true);
        }
    }

    /// <summary>
    /// Repositions and reorients the arena according to the left hand or right hand 
    /// position, and according to the head (AKA main camera) position. (Really this
    /// is achieved by transforming the camera and controllers--the arena doesn't move.)
    /// </summary>
    private void MoveArena(bool usingLeftHand) {
        Vector3 headPos = GameObject.FindGameObjectWithTag("MainCamera").transform.position;
        GameObject hand = usingLeftHand ?
            GameObject.FindGameObjectWithTag("LeftHand") :
            GameObject.FindGameObjectWithTag("RightHand");
        Vector3 handPos = hand.transform.position;
        GameObject cameraOffset = GameObject.FindGameObjectWithTag("CameraOffset");

        // First make the position of the hand match the center of the tunnel opening.

        Bounds tunnelBounds = Utils.GetBoundingBox(GameObject.FindGameObjectWithTag("Tunnel"));
        Vector3 newHandPos = new Vector3(
            tunnelBounds.center.x, 
            tunnelBounds.center.y, 
            tunnelBounds.min.z);
        Vector3 necessaryHandShift = newHandPos - handPos;
        cameraOffset.transform.position += necessaryHandShift;

        // Finally rotate around the vertical axis through the hand position to make
        // the plane through the player's head and hand go through the middle of the
        // tunnel. (Take advantage of the fact that the tunnel is parallel to the z-axis
        // and has x-component 0 at its center, so the hand-to-head vector should have 
        // x-component 0.)

        Vector3 handToHead = headPos - newHandPos;
        float angleOff = Utils.AngleOffAroundAxis(handToHead, new Vector3(0, 0, -1), Vector3.up);
        cameraOffset.transform.RotateAround(newHandPos, Vector3.up, -angleOff);
    }    
}
