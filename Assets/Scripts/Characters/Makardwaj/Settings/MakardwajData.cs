using UnityEngine;

namespace Makardwaj.Characters.Makardwaj.Data
{
    [CreateAssetMenu(fileName = "MakardwajData", menuName = "Data/Characters/Makardwaj", order = 1)]
    public class MakardwajData : ScriptableObject
    {
        public string characterName = "Dhwaj";

        [Header("Locomotion")]
        public float walkSpeed = 5f;
        public float jumpSpeed = 7f;

        [Header("Ground Check")]
        public LayerMask groundLayer;
        public float groundCheckRadius = 1f;

        [Header("In Air State")]
        public int amountOfJumps = 1;
        public float coyoteTime = 0.2f;

        [Header("Bubble")]
        public int maxBubbleCount = 1;
        public int bubblePoolStartingCount = 2;

        [Header("Debuff")]
        public int debuffTime = 6;
        public Color debuffColor;
    }
}
