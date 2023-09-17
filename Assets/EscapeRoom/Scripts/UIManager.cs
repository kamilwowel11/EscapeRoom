using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public UIDialogue uiDialogue;

    [SerializeField]
    private GameObject mainMenu;
    [SerializeField]
    private GameObject interactionGameObject;
    [SerializeField]
    private TextMeshProUGUI interactionText;
    [SerializeField]
    private GameObject startGameText;
    [SerializeField]
    private GameObject credits;

    [SerializeField]
    private Image coinImage;

    [SerializeField]
    private float coinNotPickedUpAlpha;

    private Color coinColor;
    private bool isMenuOpened = false;

    private void Awake()
    {
        coinColor = coinImage.color;
    }

    public void UpdateInteractionText(IInteractable interactionTarget, bool isInDialogue)
    {
        if (interactionTarget == null || isInDialogue == true)
        {
            interactionGameObject.SetActive(false);
            return;
        }
        else
        {
            interactionText.text = interactionTarget.interactTextUI;
            interactionGameObject.SetActive(true);
        }
    }

    public void UpdateCoinState(bool active)
    {
        if ((active && coinColor.a == 1) || (!active && coinColor.a == coinNotPickedUpAlpha))
            return;

        if (active)
        {
            Color newColor = new Color(coinColor.r, coinColor.g, coinColor.b, 1);
            coinImage.color = newColor;
        }
        else
        {
            Color newColor = new Color(coinColor.r, coinColor.g, coinColor.b, coinNotPickedUpAlpha);
            coinImage.color = newColor;
        }
    }

    public void OnStartGame()
    {
        startGameText.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;
    }

    public void OpenMenu(InputAction.CallbackContext context)
    {
        isMenuOpened = !isMenuOpened;
        Cursor.lockState = isMenuOpened ? CursorLockMode.None : CursorLockMode.Locked;
        mainMenu.SetActive(isMenuOpened);
    }

    public void ResumeGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        isMenuOpened = false;
        mainMenu.SetActive(isMenuOpened);
    }

    public void RollCredits()
    {
        credits.SetActive(true);
    }
}
