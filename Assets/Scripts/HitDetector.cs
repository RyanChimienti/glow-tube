using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Invoked when the player or opponent hits the ball. Note that
/// multiple hits may occur in quick succession for what looks like
/// a single hit.
/// </summary>
[System.Serializable]
public class BallHitEvent : UnityEvent<bool> {}; 

/// <summary>
/// Detects when the player or opponent hits the ball.
/// </summary>
public class HitDetector : MonoBehaviour {
    [Header("Invoked when the player or opponent hits the ball " +
        "(argument is true if player, false if opponent)")]
    [SerializeField]
    public BallHitEvent BallHitEvent = new BallHitEvent();

    void OnCollisionEnter(Collision collision) {
        string colliderTag = collision.collider.gameObject.tag;
        if (colliderTag == "Paddle" || colliderTag == "Opponent") {
            BallHitEvent.Invoke(colliderTag == "Paddle");
        }
    }
}
