using Makardwaj.Bosses;
using Makardwaj.Common;
using Makardwaj.Common.FiniteStateMachine;

namespace Makardwaj.Bosses
{
    public class BaseFrogBossState : State
    {
        protected FrogBoss _frogBoss;
        protected FrogBossData _frogBossData;

        public BaseFrogBossState(Controller controller, StateMachine stateMachine, BaseData playerData, string animBoolName, string sfxName = "") : base(controller, stateMachine, playerData, animBoolName, sfxName)
        {
            _frogBoss = controller as FrogBoss;
            _frogBossData = playerData as FrogBossData;
        }
    }
}