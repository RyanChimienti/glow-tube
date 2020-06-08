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
    /// If the ball hits the tunnel wall this many times on a turn, that
    /// is an immediate loss.
    /// </summary>
    public static int NUM_BOUNCES_FOR_LOSS = 7;

    /// <summary>
    /// The color the ball starts at the beginning of each turn.
    /// </summary>
    public static Color BALL_START_COLOR = Color.white;

    /// <summary>
    /// The ball gradually turns this color as it accrues wall bounces over 
    /// the course of a turn. It gets all the way to this color when a
    /// loss is triggered from too many bounces.
    /// </summary>
    public static Color BOUNCE_LOSS_COLOR = Color.red;

    /// <summary>
    /// If any part of the paddle exceeds this Z value, the paddle is
    /// disabled. (This prevents the player from reaching too far
    /// through the tunnel.) NOTE: This is not yet used.
    /// </summary>
    private static float PADDLE_BOUNDARY_Z = 0.8f;
}
