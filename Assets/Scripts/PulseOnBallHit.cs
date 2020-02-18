using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulseOnBallHit : MonoBehaviour {
    public Material pulseMaterial;
    public Material originalMaterial;

    private void Start() {
        this.originalMaterial = this.GetComponent<MeshRenderer>().GetMaterials();
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.collider.gameObject.tag == "Ball") {
            this.GetComponent<MeshRenderer>().materials[0] = ;
        }
    }

    void OnCollisionExit(Collision collision) {
        if (collision.collider.gameObject.tag == "Ball") {

        }
    }
}
