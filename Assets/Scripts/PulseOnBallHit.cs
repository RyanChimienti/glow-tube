using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulseOnBallHit : MonoBehaviour {
    public Material pulseMaterial;
    public AudioSource pulseSound;
    private Material[] originalMaterials;

    private void Start() {
        // Save the original material to set it back when the pulse ends.
        MeshRenderer renderer = this.GetComponent<MeshRenderer>();
        this.originalMaterials = renderer.materials;
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.collider.gameObject.tag == "Ball") {
            MeshRenderer meshRenderer = this.GetComponent<MeshRenderer>();
            Material[] oldMats = meshRenderer.materials;
            oldMats[0] = pulseMaterial;
            meshRenderer.materials = oldMats;
            pulseSound.Play();
        }
    }

    void OnCollisionExit(Collision collision) {
        if (collision.collider.gameObject.tag == "Ball") {
            this.GetComponent<MeshRenderer>().materials = originalMaterials;
        }
    }
}
