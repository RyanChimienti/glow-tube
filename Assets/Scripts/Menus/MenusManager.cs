using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenusManager : MonoBehaviour {
    public GameObject PlayMenu;
    public GameObject SettingsMenu;

    private void Start() {
        ShowPlayMenu();
    }

    public void OnRoundStart() {
        CloseAllMenus();
    }

    public void OnPlayMenuSettingsButtonPress() {
        ShowSettingsMenu();
    }

    public void OnSettingsMenuDoneButtonPress() {
        ShowPlayMenu();
    }

    public void OnRoundEnd(bool playerWon, OutcomeReason reason) {
        StartCoroutine(ShowPlayMenuDelayed(GameConstants.RETURN_TO_MENU_DELAY));
    }

    private IEnumerator ShowPlayMenuDelayed(float delay) {
        yield return new WaitForSeconds(delay);
        ShowPlayMenu();
    }

    private void CloseAllMenus() {
        PlayMenu.SetActive(false);
        SettingsMenu.SetActive(false);
    }

    private void ShowPlayMenu() {
        PlayMenu.SetActive(true);
        SettingsMenu.SetActive(false);
    }

    private void ShowSettingsMenu() {
        SettingsMenu.SetActive(true);
        PlayMenu.SetActive(false);        
    }
}
