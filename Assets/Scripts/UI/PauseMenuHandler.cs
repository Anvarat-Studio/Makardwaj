
using UnityEngine;
using CCS.SoundPlayer;

#if UNITY_EDITOR
using UnityEditor;
#endif


namespace Makardwaj.UI
{
    public class PauseMenuHandler : MonoBehaviour
    {
        private bool _musicEnabled = true;

        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);

            Time.timeScale = (isActive) ? 0 : 1;
        }

        public void PlayClickSound()
        {
            SoundManager.Instance.PlaySFX(MixerPlayer.UI, "click");
        }

        public void QuitGame()
        {
#if !UNITY_EDITOR
            Application.Quit();
#else
            EditorApplication.isPlaying = false;
#endif
        }

        public void ToggleMusic()
        {
            _musicEnabled = !_musicEnabled;
            if (_musicEnabled)
            {
                SoundManager.Instance.UnMuteMusic();
            }
            else
            {
                SoundManager.Instance.MuteMusic();
            }
        }
    }
}