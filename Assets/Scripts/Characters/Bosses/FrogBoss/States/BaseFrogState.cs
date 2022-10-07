using System;
using Makardwaj.Common;
using Makardwaj.Common.FiniteStateMachine;

namespace Makardwaj.Bosses
{
    public class BaseFrogState : State
    {
        protected FrogBoss _frogBoss;

        public BaseFrogState(Controller controller, StateMachine stateMachine, BaseData playerData, string animBoolName, string sfxName = "") : base(controller, stateMachine, playerData, animBoolName, sfxName)
        {
            _frogBoss = controller as FrogBoss;
        }
    }
}

