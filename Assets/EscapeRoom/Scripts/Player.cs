using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerInput playerInput;
    public PlayerCameraController playerCameraController;

    [HideInInspector]
    public bool coinTaken { get; set; }

    public bool isInDialogue = false;
}
