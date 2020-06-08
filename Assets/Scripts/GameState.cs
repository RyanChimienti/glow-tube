using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Holds the state for the game as static instance variables.
/// </summary>
public static class GameState
{
    public enum Status {
        IN_MENU,
        PLAYING_ROUND
    }
    public static Status CurrentStatus;
    
    public static int PlayerScore;
    public static int OpponentScore;
    
    /// <summary>
    /// True if the player was the last one to touch the ball; false if it
    /// was the opponent.
    /// </summary>
    public static bool PlayerHitLast;

    /// <summary>
    /// The last time the ball changed possesion.
    /// </summary>
    public static System.DateTime MostRecentTurnChange;

    /// <summary>
    /// Which turn we are on, starting with 0 and increasing each time the ball
    /// changes possession.
    /// </summary>
    public static int TurnNumber;

    /// <summary>
    /// The number of times the ball has hit a tunnel wall in the current turn.
    /// </summary>
    public static int NumWallBouncesThisTurn;
}
