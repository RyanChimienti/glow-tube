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

    private Material[] originalMaterials;

    private void Start() {
        // Save the original material to set it back when the pulse ends.
        MeshRenderer renderer = this.GetComponent<MeshRenderer>();
        this.originalMaterials = renderer.materials;
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.collider.gameObject.tag == "Ball") {
            double timeSinceLastPulse = System.DateTime.Now.Subtract(lastPulseTime).TotalSeconds;            
            if (timeSinceLastPulse < MIN_TIME_BETWEEN_PULSES) {
                return;
            }

            MeshRenderer meshRenderer = this.GetComponent<MeshRenderer>();
            Material[] oldMats = meshRenderer.materials;
            oldMats[0] = pulseMaterial;
            meshRenderer.materials = oldMats;
            pulseSound.Play();

            lastPulseTime = System.DateTime.Now;

            Invoke("EndPulse", PULSE_LENGTH);
        }
    }

    private void EndPulse() {
            this.GetComponent<MeshRenderer>().materials = originalMaterials;            
    }
}
