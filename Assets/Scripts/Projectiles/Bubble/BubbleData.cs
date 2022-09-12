using Makardwaj.Common;
using UnityEngine;

namespace Makardwaj.Projectiles.Bubble.Data
{
    [CreateAssetMenu(fileName = "BubbleData", menuName = "Data/Projectiles/Bubble", order = 2)]
    public class BubbleData : BaseData
    {
        [Header("Bubble")]
        public float highSpeedDuration = 1f;
        public float highSpeed = 10f;
        public float burstTimeWithEnemyCaptured = 5f;
    }
}
