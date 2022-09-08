using UnityEngine;

namespace Makardwaj.Characters.Makardwaj.Data
{
    [CreateAssetMenu(fileName = "MakardwajData", menuName = "Data/Characters/Makardwaj", order = 1)]
    public class MakardwajData : ScriptableObject
    {
        [Header("Locomotion")]
        public float walkSpeed = 5f;
        public float jumpSpeed = 7f;
        public int maxBubbleCount = 1;

        [Header("Ground Check")]
        public LayerMask groundLayer;
        public float groundCheckRadius = 1f;
    }
}
