using Makardwaj.Characters.Makardwaj.FiniteStateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Makardwaj.Characters.Makardwaj.Utils
{
    [RequireComponent(typeof(MakardwajController))]
    public class AnimationTriggers : MonoBehaviour
    {
        private MakardwajController _makardwaj;

        private void Awake()
        {
            _makardwaj = GetComponent<MakardwajController>();
        }

        private void AnimationFinishTrigger()
        {
            _makardwaj.AnimationFinishTrigger();
        }
    }
}