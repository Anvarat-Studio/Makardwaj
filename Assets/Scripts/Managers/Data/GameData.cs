using Makardwaj.Characters.Makardwaj.FiniteStateMachine;
using UnityEngine;

namespace Makardwaj.Data
{
    [CreateAssetMenu(fileName = "GameData", menuName = "Data/GameData", order = 1)]
    public class GameData : ScriptableObject
    {
        [Header("Player Data")]
        public MakardwajController player;
        public int maxLives = 3;
        public float respawnTime = 3;
        public float splashScreenTime = 2f;
    }
}