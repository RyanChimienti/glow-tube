using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum OutcomeReason {
    BOUNCE_LOSS,
    DOUBLE_HIT,
    LET_THROUGH
}

public static class OutcomeReasonExtensions {
    public static string ToReadableString(this OutcomeReason or) {
        switch (or) {
            case OutcomeReason.BOUNCE_LOSS:
                return "Too Many Wall Bounces";
            case OutcomeReason.DOUBLE_HIT:
                return "Double Hit";
            case OutcomeReason.LET_THROUGH:
                return "Let Through";
            default:
                throw new System.Exception("OutcomeReason not recognized.");
        }
    }
}

