public interface IInteractable
{
    public bool canBeInteracted {get; set;}
    public string interactTextUI { get; set; }
    public bool CanInteract(Player player);
    public void Interact(Player player);
}
