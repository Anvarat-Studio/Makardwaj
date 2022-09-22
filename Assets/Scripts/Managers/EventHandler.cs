using UnityEngine.Events;

namespace Makardwaj.Managers
{
    public static class EventHandler
    {
        public static UnityAction<int> GameStart;
        public static UnityAction GameEnd;
        public static UnityAction<int> PlayerLiveLost;
        public static UnityAction PlayerRespawn;
        public static UnityAction<int> collectibleCollected;
        public static UnityAction EnemyKilled;
        public static UnityAction AllEnemiesKilled;
    }
}