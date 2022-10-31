using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Makardwaj.Environment
{
    public class PoisonRefill : MonoBehaviour
    {
        [SerializeField] private Animator m_anim;
        [SerializeField] private GameObject m_parent;

        private void Awake()
        {
            if(!m_anim)
            {
                m_anim = GetComponent<Animator>();
            }

            if (!m_parent)
            {
                m_parent = transform.parent.gameObject;
            }
        }

        public void Activate()
        {
            m_parent.SetActive(true);
            gameObject.SetActive(true);
        }

        public void Deactivate()
        {
            m_anim.SetBool("stop", true);
        }

        public void OnPoisonStopped()
        {
            m_anim.SetBool("stop", false);
            gameObject.SetActive(false);
            m_parent.SetActive(false);
        }
    }
}

