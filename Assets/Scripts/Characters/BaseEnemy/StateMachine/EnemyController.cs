using Makardwaj.Characters.Enemy.States;

namespace Makardwaj.Characters.Enemy.Base
{
    public class EnemyController : BaseEnemyController
    {
        #region StateMachine
        protected  override void InitializeStateMachine()
        {
            base.InitializeStateMachine();
            _stateMachine.Initialize(PatrolState);
        }
        #endregion
    }
}