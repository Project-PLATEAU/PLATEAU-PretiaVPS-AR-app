using PretiaArCloud;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Pretia.RelocChecker.UI
{
    public class RelocalizationStatusController : MonoBehaviour
    {
        [SerializeField]
        private ARSharedAnchorManager _arSharedAnchorManager;

        [SerializeField]
        private TMP_Text _statusLabel;

        [SerializeField]
        private Image _statusImage;

        private readonly Color RELOCALIZING_STATUS_COLOR = new(219f / 255f, 151f / 255f, 22f / 255f, 178 / 255f);

        private readonly Color RELOCALIZED_STATUS_COLOR = new(50 / 255f, 136 / 255f, 64 / 255f, 178 / 255f);
        
        private readonly Color FAIL_STATUS_COLOR = new(255 / 255f, 0 / 255f, 0 / 255f, 178 / 255f);
        
        private readonly Color CLEAR = new(0, 0, 0, 0);

        private bool HasLabel { get; set; }
        
        private bool HasStarted { get; set; }

#if UNITY_EDITOR
        private void Reset()
        {
            _statusLabel = GetComponentInChildren<TMP_Text>();
        }
#endif

        private void Awake()
        {
            if (_statusLabel == null)
            {
                _statusLabel = GetComponentInChildren<TMP_Text>();
                HasLabel = _statusLabel != null;
            }
            else
            {
                HasLabel = true;
            }
            _arSharedAnchorManager.OnRelocalizationStateChanged += SetLabel;
        }


        private void OnEnable()
        {
            _statusImage.color = CLEAR;
            ResetLabels();
        }

        private void OnDestroy()
        {
            _arSharedAnchorManager.OnRelocalizationStateChanged -= SetLabel;
        }

        public void ResetLabels()
        {
            HasStarted = false;
        }

        private void SetLabel(RelocalizationState newState)
        {
            switch (newState)
            {
                case RelocalizationState.Initializing:
                    if (HasLabel)
                    {
                        _statusLabel.text = "Status: INITIALIZING";
                        _statusImage.color = RELOCALIZING_STATUS_COLOR;
                        HasStarted = true;
                    }
                    break;
                
                case RelocalizationState.Relocalizing:
                    if (HasLabel)
                    {
                        _statusLabel.text = "Status: SCANNING";
                        _statusImage.color = RELOCALIZING_STATUS_COLOR;
                    }
                    break;

                case RelocalizationState.Relocalized:
                    if (HasLabel)
                    {
                        _statusLabel.text = "Status: MAP FOUND";
                        _statusImage.color = RELOCALIZED_STATUS_COLOR;
                        HasStarted = false;
                    }
                    break;
                
                case RelocalizationState.Stopped:
                    if (HasLabel && HasStarted)
                    {
                        _statusLabel.text = "Status: MAP NOT FOUND";
                        _statusImage.color = FAIL_STATUS_COLOR;
                        HasStarted = false;
                    }
                    break;
            }
        }
    }
}