using UnityEngine;
using UnityEngine.SceneManagement;

namespace Pretia.RelocChecker.Utils
{
    public class RelocalizerSceneOpener : MonoBehaviour
    {
        public void OpenRelocalizerScene()
        {
            SceneManager.LoadScene("RelocScene");
        }
    }
}