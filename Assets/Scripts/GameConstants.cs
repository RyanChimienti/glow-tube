using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Holds game values that do not change.
/// </summary>
public class GameConstants : MonoBehaviour
{
    /// <summary>
    /// If true, we log debug messages to console.
    /// </summary>
    public static bool DEBUG_MODE = false;

    /// <summary>
    /// The speed of the ball at the start of a round.
    /// </summary>
    public static float INITIAL_BALL_SPEED = 2f;

    /// <summary>
    /// The speed the ball increases by each turn.
    /// </summary>
    public static float BALL_SPEED_INCREMENT = .05f;

    /// <summary>
    /// The speed the opponent moves while playing a round.
    /// </summary>
    public static float OPPONENT_PLAY_SPEED = 1.2f;

    /// <summary>
    /// The speed the opponent moves while returning to its ready position
    /// after a round.
    /// </summary>
    public static float OPPONENT_RESET_SPEED = .5f;

    /// <summary>
    /// The amount of time (in seconds) that must have passed between
    /// two hits for them to be considered an illegal double hit.
    /// If 
    /// </summary>
    public static double DOUBLE_HIT_TOLERANCE = 0.35;

    /// <summary>
    /// If any part of the paddle exceeds this Z value, the paddle is
    /// disabled. (This prevents the player from reaching too far
    /// through the tunnel.) NOTE: This is not yet used.
    /// </summary>
    private static float PADDLE_BOUNDARY_Z = 0.8f;
}
