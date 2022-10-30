using Makardwaj.Common;
using Makardwaj.Common.FiniteStateMachine;
using UnityEngine;

namespace Makardwaj.Bosses
{
    public class FrogBossCollectPoisonState : BaseFrogBossState
    {
        private float _collectionStartTime;

        public FrogBossCollectPoisonState(Controller controller, StateMachine stateMachine, BaseData playerData, string animBoolName, string sfxName = "") : base(controller, stateMachine, playerData, animBoolName, sfxName)
        {
        }

        public override void Enter()
        {
            base.Enter();

            _collectionStartTime = Time.time;
            _frogBoss.CollectPoison();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (!isExitingState)
            {
                if (_frogBoss.IsStunned)
                {
                    Debug.Log("Stunned....");
                    stateMachine.ChangeState(_frogBoss.StunnedState); //TODO: go to stunned state
                }
                if(Time.time - _collectionStartTime >= _frogBossData.poisonCollectionTime)
                {
                    _frogBoss.StopCollectingPoison();
                    stateMachine.ChangeState(_frogBoss.GoInsidePitState);
                }
            }
        }
    }

}