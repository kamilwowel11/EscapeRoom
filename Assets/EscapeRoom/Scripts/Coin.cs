using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : Interactable, IGrabbable
{
    public bool CanInteract(Player player)
    {
        return true;
    }

    public void Interact(Player player)
    {
        if (!CanInteract(player))
            return;

        player.coinTaken = true;

        Destroy(gameObject);
    }

    public void OnGrabbed()
    {

    }
}
