using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Makardwaj.Common.Animation
{

    public class AnimationTriggers : MonoBehaviour
    {
        [SerializeField] private GameObject m_parent;
        private void AnimationStart()
        {

        }

        private void AnimationFinish()
        {
            m_parent.SetActive(false);
            gameObject.SetActive(false);
        }
    }

}