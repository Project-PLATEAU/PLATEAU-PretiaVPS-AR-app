using System.Collections.Generic;
using Pcx;
using UnityEngine;

namespace Pretia.RelocChecker.Plateau
{
    [RequireComponent(typeof(MultiModePointCloudRenderer))]
    public class PointCloudTransparencyController : MonoBehaviour
    {
        private static readonly List<PointCloudTransparencyController> ActiveComponents = new();

        public static System.Action OnElementAdded;

        [SerializeField] 
        private MultiModePointCloudRenderer pointCloudRenderer;

#if UNITY_EDITOR
        private void Reset()
        {
            pointCloudRenderer = GetComponent<MultiModePointCloudRenderer>();
        }
#endif
        
        private void OnEnable()
        {
            if (ActiveComponents.Contains(this))
            {
                return;
            }
            
            ActiveComponents.Add(this);
            OnElementAdded?.Invoke();
        }

        private void OnDisable()
        {
            if (!ActiveComponents.Contains(this))
            {
                return;
            }

            ActiveComponents.Remove(this);
        }
        
        public static void SetTransparency(float transparency)
        {
            foreach (var component in ActiveComponents)
            {
                var color = component.pointCloudRenderer.Color;
                component.pointCloudRenderer.Color = new Color(color.r, color.g, color.b, transparency);
            }
        }
    }
}