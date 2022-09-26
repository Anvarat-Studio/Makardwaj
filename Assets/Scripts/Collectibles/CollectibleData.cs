using UnityEngine;


namespace Makardwaj.Collectibles
{
    public enum CollectibleType
    {
        Banana,
        Apple,
        Orange,
        Peach
    }

    [CreateAssetMenu(fileName = "Collectible", menuName = "Data/Collectibles/Banana", order = 5)]
    public class CollectibleData : ScriptableObject
    {
        public CollectibleType type = CollectibleType.Banana;
        public int points = 1;
        public Collectible collectiblePrefab;
    }
}

