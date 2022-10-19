using Makardwaj.Common;
using UnityEngine;

namespace Makardwaj.Bosses
{
    [CreateAssetMenu(fileName = "FrogBossData", menuName = "Data/Characters/FrogBoss")]
    public class FrogBossData : BaseData
    {
        public float outOfHoleSpeed = 10f;
        public float stunTime = 5f;
        public float stompHeight = 5f;
        public float stompWaitTime = 2f;
        public float inPitWaitTime = 3f;
        public float idleTime = 3f;
        public float stompHeightVelocity = 10f;
        public float stompVelocity = 20f;
        public float stompEffectRadius = 5f;
        public float cameraShakeDuration = 0.3f;
        public float groundCheckRadius = 0.5f;
        public LayerMask groundLayer;
        public float waitBeforeEnteringPit = 1f;
    }
}