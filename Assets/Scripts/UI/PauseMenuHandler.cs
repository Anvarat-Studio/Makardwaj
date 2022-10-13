
using UnityEngine;
using CCS.SoundPlayer;

#if UNITY_EDITOR
using UnityEditor;
#endif


namespace Makardwaj.UI
{
    public class PauseMenuHandler : MonoBehaviour
    {
        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);

            Time.timeScale = (isActive) ? 0 : 1;
        }

        public void PlayClickSound()
        {
            SoundManager.Instance.PlaySFX(MixerPlayer.Instantiations, "click", 0.5f, false);
        }

        public void QuitGame()
        {
#if !UNITY_EDITOR
            Application.Quit();
#else
            EditorApplication.isPlaying = false;
#endif
        }
    }
}