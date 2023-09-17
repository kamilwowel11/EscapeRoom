using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    [SerializeField]
    private bool CanBeInteracted = true;
    public bool canBeInteracted { get => CanBeInteracted; set => canBeInteracted = value; }
    public string interactionUItext = "";
    public string interactTextUI { get => interactionUItext; set => interactionUItext = value; }
}
