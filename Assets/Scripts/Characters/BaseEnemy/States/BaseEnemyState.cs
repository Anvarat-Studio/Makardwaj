using Makardwaj.Characters.Enemy.Base;
using Makardwaj.Common;
using Makardwaj.Common.FiniteStateMachine;

public class BaseEnemyState : State
{
    protected EnemyController _enemyController;
    protected BaseEnemyData _baseEnemyData;
    public BaseEnemyState(Controller controller, StateMachine stateMachine, BaseData playerData, string animBoolName) : base(controller, stateMachine, playerData, animBoolName)
    {
        _enemyController = controller as EnemyController;
        _baseEnemyData = playerData as BaseEnemyData;
    }
}
