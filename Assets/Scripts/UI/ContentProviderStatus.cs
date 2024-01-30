using Pretia.RelocChecker.Plateau;
using TMPro;
using UnityEngine;

namespace Pretia.RelocChecker.UI
{
    public class ContentProviderStatus : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text label;

        [SerializeField]
        private PlateauContentAligner contentAligner;
        
        private void Awake()
        {
            contentAligner.ContentStatusChanged += PrintStatus;
        }

        private void PrintStatus(string status)
        {
            label.text = status;
            Debug.Log(status);
        }

        private void OnDestroy()
        {
            contentAligner.ContentStatusChanged -= PrintStatus;
        }
    }
}