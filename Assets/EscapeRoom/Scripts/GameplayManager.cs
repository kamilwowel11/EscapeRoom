using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayManager : MonoBehaviour
{
    #region Singleton
    public static GameplayManager Instance { get; private set; }
    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    #endregion

    public UIManager uiManager;
    public Player localPlayer;

    public DoorController lastDoorController;

    private void Start()
    {
        uiManager.OnStartGame();
    }
    private void Update()
    {
        uiManager.UpdateCoinState(localPlayer.coinTaken);
        uiManager.UpdateInteractionText(localPlayer.playerInput.InteractionTarget, localPlayer.isInDialogue);
    }

    public void TriggerEndGame()
    {
        Cursor.lockState = CursorLockMode.None;
        uiManager.RollCredits();
        localPlayer.playerInput.CanPlayerInput = false;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

    public void StartGame(GameObject buttonToClose)
    {
        buttonToClose.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
