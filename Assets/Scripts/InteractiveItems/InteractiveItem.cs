namespace Makardwaj.InteractiveItems
{
    public interface InteractiveItem
    {
        public abstract bool IsInteracting { get; set; }

        public void OnPlayerEnter();

        public void OnPlayerExit();

        public void Interact();
    }
}
