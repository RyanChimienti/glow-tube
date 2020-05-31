using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenuController : MonoBehaviour {
    public GameObject paddleInLeftHandToggle;

    public void SetPaddleInLeftHand(bool left) {
        if (left) {
            PlayerPrefs.SetInt("PaddleInLeftHand", 1);
        }
        else {
            PlayerPrefs.SetInt("PaddleInLeftHand", 0);
        }
    }

    public void OnEnable() {
        if (PlayerPrefs.HasKey("PaddleInLeftHand")) {
            paddleInLeftHandToggle.GetComponent<Toggle>().isOn =
                    Convert.ToBoolean(PlayerPrefs.GetInt("PaddleInLeftHand"));
        }             
    }
}
