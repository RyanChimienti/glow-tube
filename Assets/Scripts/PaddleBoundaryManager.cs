using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleBoundaryManager : MonoBehaviour {
    public AudioSource ViolationBuzz;
    
    public AudioSource MovePaddleBehindLineSound;

    public GameObject Paddle;

    public GameObject PaddleBoundaryStrip;

    public ParticleSystem ViolationParticles;

    public Material ViolationPaddleMaterial;

    public Material NormalPaddleMaterial;

    /// <summary>
    /// Whether we are currently reacting when the paddle exceeds
    /// the boundary.
    /// </summary>
    private bool _enforcingBoundary;
    
    /// <summary>
    /// Whether the paddle is currently past the boundary.
    /// </summary>
    private bool _paddleTooFarForward;

    /// <summary>
    /// The material for the paddle boundary strip.
    /// </summary>
    private Material _stripMaterial;
    
    private void Start() {
        _enforcingBoundary = true; //TODO set false here and only true on start round.
        _paddleTooFarForward = false;

        _stripMaterial = PaddleBoundaryStrip.GetComponent<MeshRenderer>().material;

        PaddleBoundaryStrip.transform.position = new Vector3(
            PaddleBoundaryStrip.transform.position.x,
            PaddleBoundaryStrip.transform.position.y,
            GameConstants.PADDLE_BOUNDARY_Z
        );
    }

    public void OnRoundStart() {
        // Wait a moment to begin enforcing in case things are repositioned
        // right after round start.
        Invoke("BeginEnforcingBoundary", .05f);
    }

    private void BeginEnforcingBoundary() {
        _enforcingBoundary = true;
    }

    public void OnRoundEnd(Boolean playerWon, OutcomeReason reason) {
        _enforcingBoundary = false;
        if (_paddleTooFarForward) {
            _paddleTooFarForward = false;
            HandleEndViolation();
        }
    }

    private void FixedUpdate() {
        if (!_enforcingBoundary) {
            return;
        }
        
        bool paddleOverBoundary = Paddle.transform.position.z > GameConstants.PADDLE_BOUNDARY_Z;
        if (paddleOverBoundary && !_paddleTooFarForward) {
            _paddleTooFarForward = true;
            HandleViolation();
        }
        else if (!paddleOverBoundary && _paddleTooFarForward) {
            _paddleTooFarForward = false;
            HandleEndViolation();
        }
    }

    /// <summary>
    /// Called when the paddle begins to violate the boundary.
    /// </summary>
    private void HandleViolation() {
        _stripMaterial.EnableKeyword("_EMISSION");
        _stripMaterial.SetColor("_EmissionColor", new Color32(40, 40, 40, 255));
        Paddle.GetComponent<MeshRenderer>().material = ViolationPaddleMaterial;
        Paddle.GetComponent<BoxCollider>().enabled = false;
        ViolationParticles.Play();
        ViolationBuzz.Play();
        MovePaddleBehindLineSound.PlayDelayed(.2f);
        StartCoroutine(KeepPlayingMoveBehindLineSound());
    }

    private IEnumerator KeepPlayingMoveBehindLineSound() {
        yield return new WaitForSeconds(.2f);

        while (true) {            
            yield return new WaitForSeconds(2f);
            MovePaddleBehindLineSound.Play();
        }        
    }

    /// <summary>
    /// Called when the paddle stops violating the boundary.
    /// </summary>
    private void HandleEndViolation() {
        StopAllCoroutines();
        _stripMaterial.SetColor("_EmissionColor", Color.black);
        Paddle.GetComponent<MeshRenderer>().material = NormalPaddleMaterial;
        Paddle.GetComponent<BoxCollider>().enabled = true;
        ViolationParticles.Stop();        
    }
}
