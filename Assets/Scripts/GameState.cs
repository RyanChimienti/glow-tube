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
}
