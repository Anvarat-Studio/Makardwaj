using Makardwaj.Characters.Enemy.States;

namespace Makardwaj.Characters.Enemy.Base
{
    public class EnemyController : BaseEnemyController
    {
        #region StateMachine
        public EnemyPatrolState PatrolState { get; private set; }

        protected  override void InitializeStateMachine()
        {
            base.InitializeStateMachine();
            PatrolState = new EnemyPatrolState(this, _stateMachine, m_enemyData, "patrol");
            _stateMachine.Initialize(PatrolState);
        }
        #endregion
    }
}