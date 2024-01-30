using Pretia.RelocChecker.Plateau;
using UnityEngine;
using UnityEngine.UI;

namespace Pretia.RelocChecker.UI
{
    public class TransparencySliderController : MonoBehaviour
    {
        [SerializeField]
        private Slider slider;

        [SerializeField] 
        private Button activationButton;

        [SerializeField] 
        private GameObject panel;

        [SerializeField] 
        private Image iconImage;

        [SerializeField] 
        private Sprite iconOn;

        [SerializeField] 
        private Sprite iconOff;
        
        
        private void Awake()
        {
            TransparencyController.OnElementAdded += OnElementAdded;
            slider.onValueChanged.AddListener(OnValueChanged);
            activationButton.onClick.AddListener(TogglePanel);
            
            panel.SetActive(false);
        }

        private void OnDestroy()
        {
            TransparencyController.OnElementAdded -= OnElementAdded;
            slider.onValueChanged.RemoveListener(OnValueChanged);
            activationButton.onClick.RemoveListener(TogglePanel);
        }

        private void OnElementAdded()
        {
            SetTransparency(slider.value);
        }

        private void OnValueChanged(float value)
        {
            SetTransparency(value);
        }

        private void TogglePanel()
        {
            panel.SetActive(!panel.activeSelf);
        }
        
        private void SetTransparency(float value)
        {
            TransparencyController.SetTransparency(value);
            iconImage.sprite = value <= 0 ? iconOff : iconOn;
        }
    }
}