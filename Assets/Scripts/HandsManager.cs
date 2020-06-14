using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HandsManager : MonoBehaviour {    
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

    public void ToggleControllersActive() {
        SetControllersActive(!_controllersActive);
    }

    private void SetControllersActive(bool active) {
        // Before activating the paddle, make sure it's in the correct hand.
        if (!active && PlayerPrefs.HasKey("PaddleInLeftHand")) {
            paddle.GetComponent<PaddleController>().LeftHand = System.Convert.ToBoolean(
                PlayerPrefs.GetInt("PaddleInLeftHand")
            );
        }
        paddle.SetActive(!active);

        leftHand.GetComponent<XRController>().hideControllerModel = !active;
        leftHand.GetComponent<XRRayInteractor>().enabled = active;
        leftControllerUI.SetActive(active);

        rightHand.GetComponent<XRController>().hideControllerModel = !active;
        rightHand.GetComponent<XRRayInteractor>().enabled = active;
        rightControllerUI.SetActive(active);

        _controllersActive = active;
    }
}
