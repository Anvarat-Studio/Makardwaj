using UnityEngine;

namespace Makardwaj.Common
{
    public abstract class Controller : MonoBehaviour
    {
        private Animator _anim;

        public Animator Anim
        {
            get
            {
                if (!_anim)
                    _anim = GetComponentInChildren<Animator>();

                return _anim;
            }
        }
    }
}