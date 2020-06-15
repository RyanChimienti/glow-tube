using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Fires events on controller button presses.
/// </summary>
public class ControllerInputDetector : MonoBehaviour {
    [Header("Fires every frame the left secondary button is held.")]
    public UnityEvent LeftSecondaryButtonHeld = new UnityEvent();

    [Header("Fires every frame the right secondary button is held.")]
    public UnityEvent RightSecondaryButtonHeld = new UnityEvent();

    [Header("Fires once as the left secondary button goes down.")]
    public UnityEvent LeftSecondaryButtonDown = new UnityEvent();

    [Header("Fires once as the right secondary button goes down.")]
    public UnityEvent RightSecondaryButtonDown = new UnityEvent();

    [Header("Fires once as the left secondary button comes up.")]
    public UnityEvent LeftSecondaryButtonUp = new UnityEvent();

    [Header("Fires once as the right secondary button comes up.")]
    public UnityEvent RightSecondaryButtonUp = new UnityEvent();

    /// <summary>
    /// The left controller, if we can find it.
    /// </summary>
    private UnityEngine.XR.InputDevice _leftController;

    /// <summary>
    /// The right controller, if we can find it.
    /// </summary>
    private UnityEngine.XR.InputDevice _rightController;

    /// <summary>
    /// Whether the left secondary button is currently pressed.
    /// </summary>
    private bool _leftSecondaryButtonIsPressed = false;

    /// <summary>
    /// Whether the right secondary button is currently pressed.
    /// </summary>
    private bool _rightSecondaryButtonIsPressed = false;

    private void FixedUpdate() {
        bool leftControllerReady = TryUpdateController(true);
        bool rightControllerReady = TryUpdateController(false);

        bool leftSecondaryNowPressed = leftControllerReady && SecondaryButtonIsPressed(true);
        if (leftSecondaryNowPressed) {
            if (!_leftSecondaryButtonIsPressed) {
                _leftSecondaryButtonIsPressed = true;
                LeftSecondaryButtonDown.Invoke();
            }

            LeftSecondaryButtonHeld.Invoke();            
        }
        else if (_leftSecondaryButtonIsPressed) {
            _leftSecondaryButtonIsPressed = false;
            LeftSecondaryButtonUp.Invoke();
        }

        bool rightSecondaryNowPressed = rightControllerReady && SecondaryButtonIsPressed(false);
        if (rightSecondaryNowPressed) {
            if (!_rightSecondaryButtonIsPressed) {
                _rightSecondaryButtonIsPressed = true;
                RightSecondaryButtonDown.Invoke();
            }

            RightSecondaryButtonHeld.Invoke();
        }
        else if (_rightSecondaryButtonIsPressed) {
            _rightSecondaryButtonIsPressed = false;
            RightSecondaryButtonUp.Invoke();
        }
    }

    /// <summary>
    /// Attempts to update the reference to the specified controller, returning
    /// true if the reference is now valid, and false if not.
    /// </summary>
    private bool TryUpdateController(bool left) {
        // Do nothing if the controller is already valid.
        UnityEngine.XR.InputDevice curController = left ? _leftController : _rightController;
        if (curController.isValid) {
            return true;
        }

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

        if (left) {
            _leftController = controllers[0];
        }
        else {
            _rightController = controllers[0];
        }
        return true;
    }

    /// <summary>
    /// Returns whether the secondary button is pressed on the left or right
    /// controller (Y or B respectively on the Quest).
    /// </summary>
    private bool SecondaryButtonIsPressed(bool left) {
        UnityEngine.XR.InputDevice controller = left ? _leftController : _rightController;

        if (controller.TryGetFeatureValue(
                UnityEngine.XR.CommonUsages.secondaryButton,
                out bool secondaryButtonValue
           )
        ) {
            return secondaryButtonValue;
        }

        // If we can't get whether the button is pressed, assume it's not.
        return false;
    }
}
