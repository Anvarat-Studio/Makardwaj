using UnityEngine;


namespace Makardwaj.Collectibles
{
    [CreateAssetMenu(fileName = "Collectible", menuName = "Data/Collectibles/Banana", order = 5)]
    public class CollectibleData : ScriptableObject
    {
        public string collectibleName = "Banana";
        public float points = 1f;
        public GameObject collectiblePrefab;
    }
}

