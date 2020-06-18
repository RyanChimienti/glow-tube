using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Holds game values that do not change.
/// </summary>
public class GameConstants {
    /// <summary>
    /// If true, we log debug messages to console.
    /// </summary>
    public static bool DEBUG_MODE = true;

    /// <summary>
    /// The greatest possible angle off from straight when the ball
    /// is launched towards the player (angle is chosen randomly
    /// under this constraint).
    /// </summary>
    public static float MAX_BALL_START_ANGLE = 10;

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
    /// The color of the ball when it shatters for various reasons.
    /// </summary>
    public static IReadOnlyDictionary<OutcomeReason, Color> BALL_SHATTER_COLORS
    = new Dictionary<OutcomeReason, Color> {
           { OutcomeReason.BOUNCE_LOSS, Color.magenta },
           { OutcomeReason.DOUBLE_HIT, Color.yellow },
           { OutcomeReason.LET_THROUGH, Color.cyan }
    };

    /// <summary>
    /// The color of the tunnel at the start of the game, and in between rounds.
    /// </summary>
    public static Color TUNNEL_START_COLOR = new Color32(77, 77, 77, 255);

    /// <summary>
    /// The color of the tunnel when the player hit the ball last.
    /// </summary>
    public static Color PLAYER_TURN_TUNNEL_COLOR = new Color32(0, 70, 0, 255);

    /// <summary>
    /// The color of the tunnel when the opponent hit the ball last.
    /// </summary>
    public static Color OPPONENT_TURN_TUNNEL_COLOR = new Color32(154, 63, 0, 255);

    /// <summary>
    /// The time (in seconds) that we wait after the end of a round before
    /// auditorily announcing the outcome.
    /// </summary>
    public static float OUTCOME_ANNOUNCEMENT_DELAY = 1.0f;

    /// <summary>
    /// The time (in seconds) that we wait between the end of a round and
    /// returning to the menu. This time is for end of round graphical effects 
    /// (like the ball shattering) and announcing the round outcome.
    /// </summary>
    public static float RETURN_TO_MENU_DELAY = 3.0f;

    /// <summary>
    /// If any part of the paddle exceeds this Z value, the paddle is
    /// disabled. (This prevents the player from reaching too far
    /// through the tunnel.) NOTE: This is not yet used.
    /// </summary>
    private static float PADDLE_BOUNDARY_Z = 0.8f;
}
