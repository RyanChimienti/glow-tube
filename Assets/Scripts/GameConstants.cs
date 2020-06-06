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
    public static bool DEBUG_MODE = true;

    /// <summary>
    /// The amount of time (in seconds) that must have passed between
    /// two hits for them to be considered an illegal double hit.
    /// If 
    /// </summary>
    public static double DOUBLE_HIT_TOLERANCE = 0.3;

    /// <summary>
    /// If any part of the paddle exceeds this Z value, the paddle is
    /// disabled. (This prevents the player from reaching too far
    /// through the tunnel.) NOTE: This is not yet used.
    /// </summary>
    private static float PADDLE_BOUNDARY_Z = 0.8f;
}
