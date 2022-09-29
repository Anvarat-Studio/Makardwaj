namespace Makardwaj.Characters.Enemy.Base
{
    public class EnemyFrogController : BaseEnemyController
    {
        #region StateMachine
        protected override void InitializeStateMachine()
        {
            base.InitializeStateMachine();
            _stateMachine.Initialize(PatrolState);
        }
        #endregion
    }
}