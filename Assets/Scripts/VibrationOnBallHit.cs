using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Makes the controller holding the paddle vibrate when the paddle hits the ball.
/// </summary>
public class VibrationOnBallHit : MonoBehaviour {
    /// <summary>
    /// The time the vibration lasts in seconds.
    /// </summary>
    private static float VIBRATION_DURATION = 0.1f;

    /// <summary>
    /// The intensity of the vibration (from 0.0 to 1.0)
    /// </summary>
    private static float VIBRATION_AMPLITUDE = 0.5f;

    /// <summary>
    /// The amount of time (in seconds) that must have elapsed
    /// since the previous vibration to have a new vibration.
    /// </summary>
    private static double MIN_TIME_BETWEEN_VIBRATIONS = GameConstants.DOUBLE_HIT_TOLERANCE;

    /// <summary>
    /// The last time the controller vibrated.
    /// </summary>
    private System.DateTime lastVibrationTime = System.DateTime.MinValue;

    private UnityEngine.XR.InputDevice _controller;

    private void OnEnable() {
        bool paddleInLeftHand = System.Convert.ToBoolean(PlayerPrefs.GetInt("PaddleInLeftHand"));

        var controllers = new List<UnityEngine.XR.InputDevice>();
        UnityEngine.XR.InputDevices.GetDevicesAtXRNode(
            paddleInLeftHand ? UnityEngine.XR.XRNode.LeftHand : UnityEngine.XR.XRNode.RightHand,
            controllers
        );

        if (controllers.Count != 1) {
            string handType = paddleInLeftHand ? "left" : "right";
            throw new System.Exception($"Error: Found {controllers.Count} {handType} hands.");
        }
        _controller = controllers[0];

        // Make sure the controller supports haptics, or else we can't do vibrations.
        UnityEngine.XR.HapticCapabilities capabilities;
        if (!_controller.TryGetHapticCapabilities(out capabilities) || !capabilities.supportsImpulse) {
            this.enabled = false;
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.collider.gameObject.tag == "Ball") {
            double timeSinceLastVibration = System.DateTime.Now.Subtract(lastVibrationTime).TotalSeconds;
            if (timeSinceLastVibration < MIN_TIME_BETWEEN_VIBRATIONS) {
                return;
            }

            _controller.SendHapticImpulse(0, VIBRATION_AMPLITUDE, VIBRATION_DURATION);
            lastVibrationTime = System.DateTime.Now;
        }        
    }
}
