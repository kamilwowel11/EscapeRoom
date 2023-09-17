using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCCoinMaster : Interactable, IInteractable
{
    [SerializeField]
    private Animator animator;

    [SerializeField]
    private GameObject questionMarkObject;

    [Header("Ink dialogue"), SerializeField]
    private TextAsset inkJSON_NoCoin;
    [SerializeField]
    private TextAsset inkJSON_Coin;

    private void Awake()
    {
        questionMarkObject.SetActive(true);
    }

    public bool CanInteract(Player player)
    {
        return true;
    }

    public void Interact(Player player)
    {
        if (!CanInteract(player))
            return;

        questionMarkObject.SetActive(false);

        if (player.isInDialogue)
        {
            GameplayManager gameplayInstance = GameplayManager.Instance;

            if (!gameplayInstance.uiManager.uiDialogue.ContinueStory())
            {
                player.isInDialogue = false;
                if (player.coinTaken && gameplayInstance.uiManager.uiDialogue.lastChoiceTaken != "No")
                {
                    gameplayInstance.lastDoorController.Interact(player);
                }
                else
                {
                    Debug.Log("End of dialogue.");
                }
                animator.SetBool("IsTalking", false);
            }
                
            return;
        }
        else
        {
            player.isInDialogue = true;
            TextAsset currentTextAsset;

            animator.SetBool("IsTalking", true);

            if (player.coinTaken)
            {
                currentTextAsset = inkJSON_Coin;
            }
            else
            {
                currentTextAsset = inkJSON_NoCoin;
            }

            GameplayManager.Instance.uiManager.uiDialogue.StartNewDialogue(currentTextAsset);
        }
    }
}
