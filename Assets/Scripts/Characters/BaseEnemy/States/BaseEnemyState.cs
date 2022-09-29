using Makardwaj.Characters.Enemy.Base;
using Makardwaj.Common;
using Makardwaj.Common.FiniteStateMachine;

public class BaseEnemyState : State
{
    protected BaseEnemyController _enemyController;
    protected BaseEnemyData _baseEnemyData;

    protected bool _isGrounded;
    public BaseEnemyState(Controller controller, StateMachine stateMachine, BaseData playerData, string animBoolName) : base(controller, stateMachine, playerData, animBoolName)
    {
        _enemyController = controller as BaseEnemyController;
        _baseEnemyData = playerData as BaseEnemyData;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void DoChecks()
    {
        base.DoChecks();

        _isGrounded = _enemyController.CheckIfGrounded();
    }
}
