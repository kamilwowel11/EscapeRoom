using UnityEngine;
using UnityEngine.UI;

public class DoorController : Interactable, IInteractable
{
    [SerializeField]
    private Animation doorAnim;

    [SerializeField]
    private bool takeCoinOnOpen = false;

    private bool isMetRequirements = false;

    DoorState doorState = new DoorState();

    private void Start()
    {
        doorState = DoorState.Closed;
    }

    public bool CanInteract(Player player)
    {
        if (player.coinTaken || isMetRequirements)
            return true;
        else
            return false;
    }

    public void Interact(Player player)
    {
        if (!CanInteract(player))
            return;

        InteractDoor(player);
    }

    private void InteractDoor(Player player)
    {
        if (doorState == DoorState.Closed && !doorAnim.isPlaying)
        {
            doorAnim.Play("Door_Open");
            doorState = DoorState.Opened;
            if (takeCoinOnOpen && isMetRequirements == false)
            {
                player.coinTaken = false;
            }
            isMetRequirements = true;
        }

        if (doorState == DoorState.Opened && !doorAnim.isPlaying)
        {
            doorAnim.Play("Door_Close");
            doorState = DoorState.Closed;
        }
    }

    enum DoorState
    {
        Closed,
        Opened,
        Jammed
    }
}
