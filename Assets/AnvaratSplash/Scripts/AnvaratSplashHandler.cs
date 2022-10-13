using UnityEngine;
using UnityEngine.Events;

namespace Anvarat.Splash
{
    public class AnvaratSplashHandler : MonoBehaviour
    {
        [SerializeField] private Animator m_animator;

        public UnityAction splashStarted;
        public UnityAction splashEnded;
        public UnityAction splashScreenActivated;

        public void SplashScreenStartTrigger()
        {
            //m_animator.SetBool("disable", false);
            splashStarted?.Invoke();
        }

        public void SplashScreenActive()
        {
            splashScreenActivated?.Invoke();
        }

        public void SplashScreenEndTrigger()
        {
            splashEnded?.Invoke();
            gameObject.SetActive(false);
        }

        public void DisableSplash()
        {
            m_animator.SetBool("disable", true); 
        }
    }
}