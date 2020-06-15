using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// Fired when the player's hands change (i.e. from controllers to a paddle).
/// </summary>
[System.Serializable]
public class HandsChangeEvent : UnityEvent<bool> {}

public class HandsManager : MonoBehaviour {
    [Header("Fired when the player's hands change (true if controllers, false if paddle).")]
    public HandsChangeEvent HandsChangeEvent = new HandsChangeEvent();

    public GameObject leftHand;
    public GameObject rightHand;
    public GameObject leftControllerUI;
    public GameObject rightControllerUI;
    public GameObject paddle;

    /// <summary>
    /// True if the player is holding controllers; false if
    /// the player is holding the paddle.
    /// </summary>
    private bool _controllersActive = true;

    void Start() {
        paddle.SetActive(false);
    }

    public void OnRoundStart() {
        SetControllersActive(false);
    }

    public void OnRoundEnd(bool playerWon, OutcomeReason reason) {
        StartCoroutine(SetControllersActiveDelayed(GameConstants.RETURN_TO_MENU_DELAY, true));
    }

    private IEnumerator SetControllersActiveDelayed(float delay, bool active) {
        yield return new WaitForSeconds(delay);
        SetControllersActive(active);
    }

    private void SetControllersActive(bool active) {
        paddle.SetActive(!active);

        leftHand.GetComponent<XRController>().hideControllerModel = !active;
        leftHand.GetComponent<XRRayInteractor>().enabled = active;
        leftControllerUI.SetActive(active);

        rightHand.GetComponent<XRController>().hideControllerModel = !active;
        rightHand.GetComponent<XRRayInteractor>().enabled = active;
        rightControllerUI.SetActive(active);

        _controllersActive = active;

        HandsChangeEvent.Invoke(active);
    }
}
