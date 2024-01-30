using UnityEngine;

namespace Pretia.RelocChecker.UI
{
    public class SafeArea : MonoBehaviour
    {
        [SerializeField]
        [Range(0f, 1f)]
        private float topSafety = 1;

        [SerializeField]
        [Range(0f, 1f)]
        private float bottomSafety = 1;

        [SerializeField]
        private float topPadding;

        [SerializeField]
        private float bottomPadding;

        public int TopOffset { get; set; }
        public int BottomOffset { get; set; }

        private void Awake()
        {
            var rectTransform = transform as RectTransform;
            if (rectTransform == null) { return; }

            var defaultAnchorMin = rectTransform.anchorMin;
            var defaultAnchorMax = rectTransform.anchorMax;

            var safeArea = Screen.safeArea;
            var anchorMin = safeArea.position;
            var anchorMax = anchorMin + safeArea.size;

            anchorMin.x /= Screen.width;
            anchorMax.x /= Screen.width;
            anchorMin.y /= Screen.height;
            anchorMax.y /= Screen.height;

            var safeAnchorMin = Vector2.Lerp(anchorMin, defaultAnchorMin, 1 - bottomSafety);
            var safeAnchorMax = Vector2.Lerp(anchorMax, defaultAnchorMax, 1 - topSafety);

            safeAnchorMin += Vector2.up * bottomPadding / Screen.height;
            safeAnchorMax -= Vector2.up * topPadding / Screen.height;

            rectTransform.anchorMin = safeAnchorMin;
            rectTransform.anchorMax = safeAnchorMax;

            TopOffset = Mathf.CeilToInt(
                (safeAnchorMax.y - defaultAnchorMax.y) * Screen.height
            );

            BottomOffset = Mathf.CeilToInt(
                (safeAnchorMin.y - defaultAnchorMin.y) * Screen.height
            );
        }
    }
}