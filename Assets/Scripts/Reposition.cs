using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reposition : MonoBehaviour {
    public void Update() {
        // We can't reposition unless we're in the menu.
        if (GameState.CurrentStatus != GameState.Status.IN_MENU) {
            return;
        }

        bool rightHandRepositioning = SecondaryButtonIsPressed(false);
        if (rightHandRepositioning) {
            MoveArena(false);
            return;
        }

        bool leftHandRepositioning = SecondaryButtonIsPressed(true);
        if (leftHandRepositioning) {
            MoveArena(true);
        }
    }

    /// <summary>
    /// Returns whether the secondary button is pressed on the left or right
    /// controller (Y or B respectively on the Quest).
    /// </summary>
    private bool SecondaryButtonIsPressed(bool left) {
        var controllers = new List<UnityEngine.XR.InputDevice>();
        UnityEngine.XR.InputDevices.GetDevicesAtXRNode(
            left ? UnityEngine.XR.XRNode.LeftHand : UnityEngine.XR.XRNode.RightHand,
            controllers
        );

        if (controllers.Count == 0) {
            return false;
        }
        if (controllers.Count > 1) {
            string handType = left ? "left" : "right";
            throw new System.Exception($"Error: Found multiple {handType} hands.");
        }

        UnityEngine.XR.InputDevice controller = controllers[0];

        bool secondaryButtonValue;
        if (controller.TryGetFeatureValue(
                UnityEngine.XR.CommonUsages.secondaryButton,
                out secondaryButtonValue
           )
            && secondaryButtonValue
        ) {
            return true;
        }
        return false;
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

        Bounds tunnelBounds = GetBoundingBox(GameObject.FindGameObjectWithTag("Tunnel"));
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

    /// <summary>
    /// Returns the bounding box of an object's colliders, relative to the standard axes.
    /// Takes into account the object itself and all its descendants. Assumes the position of
    /// the object's transform is within the bounding box. 
    /// </summary>
    private Bounds GetBoundingBox(GameObject obj) {
        Bounds boundingBox = new Bounds(obj.transform.position, Vector3.zero);
        Collider[] colliders = obj.GetComponentsInChildren<Collider>(true);
        foreach (Collider col in colliders) {
            boundingBox.Encapsulate(col.bounds);
        }
        Debug.DrawLine(boundingBox.min, boundingBox.max, Color.red);
        return boundingBox;
    }
}
