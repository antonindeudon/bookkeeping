using UnityEngine;
using UnityEngine.SceneManagement;

namespace Bookkeeping
{
    public static class Utility
    {
        public static void QuitApplication()
        {
#if UNITY_EDITOR
            // Application.Quit() does not work in the editor so
            // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
