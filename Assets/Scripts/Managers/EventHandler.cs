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
        public static UnityAction<int> ResetLives;
        public static UnityAction<string, bool> LevelComplete;
        public static UnityAction LevelChangeStarted;
        public static UnityAction LevelStarted;
        public static UnityAction LevelTextDisabled;
        public static UnityAction<int> bossTookDamage;
        public static UnityAction heavenActivated;
        public static UnityAction heavenDeactivated;
        public static UnityAction heavenMainDialogueCompleted;
        public static UnityAction bossDead;
    }
}