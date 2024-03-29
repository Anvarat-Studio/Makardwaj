﻿using Makardwaj.Common;
using Makardwaj.Common.FiniteStateMachine;

namespace Makardwaj.Bosses
{
    public class FrogBossIdleState : BaseFrogBossState
    {
        public FrogBossIdleState(Controller controller, StateMachine stateMachine, BaseData playerData, string animBoolName, string sfxName = "") : base(controller, stateMachine, playerData, animBoolName, sfxName)
        {
        }
    }
}

