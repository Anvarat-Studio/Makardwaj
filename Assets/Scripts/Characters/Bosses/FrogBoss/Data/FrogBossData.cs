using Makardwaj.Common;
using UnityEngine;

namespace Makardwaj.Bosses
{
    [CreateAssetMenu(fileName = "FrogBossData", menuName = "Data/Characters/FrogBoss")]
    public class FrogBossData : BaseData
    {
        public float idleWaitTime = 2f;
        public float stunnedTime = 2f;
        public float coolDownTime = 5f;
        public float jumpAngle = 45f;
        public float jumpSpeed = 7f;
        public float arcHeight = 0.3f;
        public LayerMask enemyLayer;
        public float tongueLength = 4f;
    }
}