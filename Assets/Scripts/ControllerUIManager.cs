using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControllerUIManager : MonoBehaviour {
    public Image secondaryButtonIcon;
    public Image secondaryButtonRing;
    public bool isLeft;

    private IEnumerator _secondaryButtonBlinkRoutine;

    private void OnEnable() {
        StartCoroutine(BlinkSecondaryButton());
    }

    public void OnRepositionStart(bool isLeftHand) {
        if (isLeftHand == this.isLeft) {
            StopAllCoroutines();
            secondaryButtonRing.color = Color.cyan;
            secondaryButtonIcon.color = Color.cyan;
        }        
    }

    public void OnRepositionEnd(bool isLeftHand) {
        // Even if it was the other hand that stopped repositioning,
        // we reset the blinking to give the hands the same cadence.
        // TODO since the hands have to think about each other anyway,
        // someday have a single UI manager that governs both hands.
        StopAllCoroutines();
        StartCoroutine(BlinkSecondaryButton());
    }

    private IEnumerator BlinkSecondaryButton() {
        secondaryButtonIcon.color = Color.white;
        while (true) {
            secondaryButtonRing.color = Color.white;
            yield return new WaitForSeconds(0.5f);
            secondaryButtonRing.color = Color.gray;
            yield return new WaitForSeconds(0.5f);
        }
    }
}
