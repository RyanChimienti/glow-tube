using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Governs the shattering behavior for the ball.
/// </summary>
public class BallShatter : MonoBehaviour {
    public GameObject ball;

    [Tooltip("The prefab to use for the shattered ball.")]
    public GameObject ShatteredBallPrefab;

    [Tooltip("The GameObject containing (in its children) the ball shatter sound and particles.")]
    public GameObject BallShatterEffects;

    private GameObject _shatteredBall;

    public void OnRoundEnd(bool playerWon, OutcomeReason reason) {
        Color shatterColor = GameConstants.BALL_SHATTER_COLORS[reason];
        
        BallShatterEffects.transform.position = ball.transform.position;
        BallShatterEffects.GetComponentInChildren<AudioSource>().Play();

        ParticleSystem particleSys = BallShatterEffects.GetComponentInChildren<ParticleSystem>();
        ParticleSystem.MainModule mainMod = particleSys.main;
        ParticleSystem.TrailModule trailMod = particleSys.trails;
        mainMod.startColor = shatterColor;
        trailMod.colorOverTrail = shatterColor;
        particleSys.Play();

        _shatteredBall = Instantiate(
            ShatteredBallPrefab,
            ball.transform.position,
            Quaternion.identity
        );

        foreach (MeshRenderer renderer in _shatteredBall.GetComponentsInChildren<MeshRenderer>()) {
            renderer.material.SetColor("_Color", shatterColor);
        }

        Vector3 ballVelocity = ball.GetComponent<Rigidbody>().velocity;
        foreach (Rigidbody r in _shatteredBall.GetComponentsInChildren<Rigidbody>()) {
            r.velocity = ballVelocity;
            r.AddExplosionForce(2.0f, _shatteredBall.transform.position, 0.2f, 0, ForceMode.Impulse);
        }
        ball.SetActive(false);

        Invoke("CleanUpShatter", GameConstants.RETURN_TO_MENU_DELAY);
    }

    public void CleanUpShatter() {
        Destroy(_shatteredBall);
        _shatteredBall = null;
    }
}
