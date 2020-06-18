using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulseOnBallHit : MonoBehaviour {
    public Material pulseMaterial;
    public AudioSource pulseSound;

    /// <summary>
    /// The length of a pulse in seconds.
    /// </summary>
    private static float PULSE_LENGTH = 0.1f;

    /// <summary>
    /// The amount of time (in seconds) that must have elapsed
    /// since the previous pulse to have a new pulse.
    /// </summary>
    private static double MIN_TIME_BETWEEN_PULSES = GameConstants.DOUBLE_HIT_TOLERANCE;

    /// <summary>
    /// The last time the object pulsed. (Really, the time of the ending of the pulse.) 
    /// We use this to discard pulses that follow previous pulses too closely.
    /// </summary>
    private System.DateTime lastPulseTime = System.DateTime.MinValue;

    /// <summary>
    /// The material of this GameObject.
    /// </summary>
    private Material _material;

    private void Start() {
        _material = this.GetComponent<MeshRenderer>().material;
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.collider.gameObject.tag == "Ball") {
            double timeSinceLastPulse = System.DateTime.Now.Subtract(lastPulseTime).TotalSeconds;            
            if (timeSinceLastPulse < MIN_TIME_BETWEEN_PULSES) {
                return;
            }
         
            _material.EnableKeyword("_EMISSION");
            _material.SetColor("_EmissionColor", new Color32(40, 40, 40, 255));
            pulseSound.Play();

            lastPulseTime = System.DateTime.Now;

            Invoke("EndPulse", PULSE_LENGTH);
        }
    }

    private void EndPulse() {
        // black is the same as no emission.
        _material.SetColor("_EmissionColor", Color.black);
    }
}
