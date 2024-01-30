using Pretia.RelocChecker.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Pretia.RelocChecker.UI
{
    public class DebugModeToggle : MonoBehaviour
    {

        [SerializeField] private Toggle debugModeToggle;

        void Start()
        {
            debugModeToggle.isOn = PlayerPrefs.GetInt(LogManager.USE_DEBUG, 0) == 1;

            debugModeToggle.onValueChanged.AddListener(SetDebugMode);
        }

        private void SetDebugMode(bool useDebugMode)
        {
            PlayerPrefs.SetInt(LogManager.USE_DEBUG, useDebugMode ? 1 : 0);
        }
    }
}
