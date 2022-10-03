
using UnityEngine;

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