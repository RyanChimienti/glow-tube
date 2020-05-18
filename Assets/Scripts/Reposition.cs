using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reposition : MonoBehaviour {
    public void Update() {
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
    /// Repositions and reorients the arena according to the left hand or right hand 
    /// position, and according to the head (AKA main camera) position.
    /// </summary>
    private bool MoveArena(bool usingLeftHand) {
        Vector3 headPos = GameObject.FindGameObjectWithTag("MainCamera").transform.position;
        Vector3 handPos = usingLeftHand ?
            GameObject.FindGameObjectWithTag("LeftHand").transform.position :
            GameObject.FindGameObjectWithTag("RightHand").transform.position;

        // TODO finish
        return false;
    }

    /// <summary>
    /// Returns whether the secondary button is pressed on the left or right
    /// controller (Y or B respectively on the Quest).
    /// </summary>
    private bool SecondaryButtonIsPressed(bool left) {
        var controllers = new List<UnityEngine.XR.InputDevice>();
        UnityEngine.XR.InputDevices.GetDevicesAtXRNode(
            UnityEngine.XR.XRNode.LeftHand,
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
}
