using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Pretia.RelocChecker.UI
{
    public class SceneExitController : MonoBehaviour
    {

        [SerializeField]
        private Button _backButton;

        [SerializeField]
        private string _sceneToLoadAfterExit;

        private void Awake()
        {
            if (_backButton != null)
                _backButton.onClick.AddListener(ExitScene);
        }

        private void OnDestroy()
        {
            Debug.Log("SceneExitController OnDestroy");
        }


        private void ExitScene()
        {

            if (!string.IsNullOrEmpty(_sceneToLoadAfterExit))
            {
                SceneManager.LoadScene(_sceneToLoadAfterExit);
            }
        }
    }
}
