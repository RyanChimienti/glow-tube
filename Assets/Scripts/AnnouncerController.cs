using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnnouncerController : MonoBehaviour {
    public AudioSource PlayerWinBounces;
    public AudioSource PlayerLoseBounces;
    public AudioSource PlayerWinLetThrough;
    public AudioSource PlayerLoseLetThrough;
    public AudioSource PlayerWinDoubleHit;
    public AudioSource PlayerLoseDoubleHit;

    /// <summary>
    /// The amount of time we wait after the end of a round to announce
    /// the outcome.
    /// </summary>
    private static float OUTCOME_DELAY = 1; 

    public void OnEndRound(bool playerWin, OutcomeReason reason) {
        AudioSource announcement;

        if (reason == OutcomeReason.BOUNCE_LOSS) {
            announcement = playerWin ? PlayerWinBounces : PlayerLoseBounces;
        }
        else if (reason == OutcomeReason.DOUBLE_HIT) {
            announcement = playerWin ? PlayerWinDoubleHit : PlayerLoseDoubleHit;
        }
        else if (reason == OutcomeReason.LET_THROUGH) {
            announcement = playerWin ? PlayerWinLetThrough : PlayerLoseLetThrough;
        }
        else {
            throw new System.Exception("Outcome reason not recognized.");
        }

        announcement.PlayDelayed(OUTCOME_DELAY);        
    }
}
