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

    public enum CollectibleColor
    {
        Red,
        Green,
        Yellow
    }

    [CreateAssetMenu(fileName = "Collectible", menuName = "Data/Collectibles/Banana", order = 5)]
    public class CollectibleData : ScriptableObject
    {
        public CollectibleType type = CollectibleType.Banana;
        public int points = 1;
        public CollectibleColor color;
        public Collectible collectiblePrefab;
    }
}

