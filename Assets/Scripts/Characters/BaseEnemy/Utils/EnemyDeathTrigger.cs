using Makardwaj.Characters.Enemy.Base;
using UnityEngine;

namespace Makardwaj.Characters.Enemy.Utils
{
    public class EnemyDeathTrigger : MonoBehaviour
    {
        private EnemyController _enemyController;
        private void Awake()
        {
            _enemyController = GetComponentInParent<EnemyController>();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (_enemyController.IsDead)
            {
                _enemyController.Die();
            }
        }
    }
}