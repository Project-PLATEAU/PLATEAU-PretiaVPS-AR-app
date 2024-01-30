using UnityEngine;
using UnityEngine.UI;

namespace Pretia.RelocChecker.UI
{
    public class CanvasScalerMatchThreshold : MonoBehaviour
    {
        [SerializeField]
        [HideInInspector]
        private CanvasScaler scaler;

        [SerializeField]
        [Range(0, 1f)]
        private float greaterAspectMath = 1;

        [SerializeField]
        [Range(0, 1f)]
        private float smallerAspectMatch = 0;

#if UNITY_EDITOR
        private void Reset()
        {
            scaler = GetComponent<CanvasScaler>();
        }
#endif

        private void Awake()
        {
            if (scaler == null)
            {
                scaler = GetComponent<CanvasScaler>();
            }

            if (scaler == null)
            {
                return;
            }

            var referenceResolution = scaler.referenceResolution;
            var referenceAspectRatio = referenceResolution.x / referenceResolution.y;

            var aspectRatio = Screen.width / (float)Screen.height;
            scaler.matchWidthOrHeight = aspectRatio <= referenceAspectRatio ? smallerAspectMatch : greaterAspectMath;
        }
    }
}