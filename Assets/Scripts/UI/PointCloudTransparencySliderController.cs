using Pretia.RelocChecker.Plateau;
using UnityEngine;
using UnityEngine.UI;

namespace Pretia.RelocChecker.UI
{
    public class PointCloudTransparencySliderController : MonoBehaviour
    {
        [SerializeField]
        private Slider slider;

        [SerializeField] 
        private GameObject pointCloudRoot;

        
        private void Awake()
        {
            TransparencyController.OnElementAdded += OnElementAdded;
            slider.onValueChanged.AddListener(OnValueChanged);
            
        }

        private void OnDestroy()
        {
            TransparencyController.OnElementAdded -= OnElementAdded;
            slider.onValueChanged.RemoveListener(OnValueChanged);
        }

        private void OnElementAdded()
        {
            SetTransparency(slider.value);
        }

        private void OnValueChanged(float value)
        {
            SetTransparency(value);
        }

        private void SetTransparency(float value)
        {
            pointCloudRoot.SetActive(value > 0);
            PointCloudTransparencyController.SetTransparency(value);
        }
    }
}