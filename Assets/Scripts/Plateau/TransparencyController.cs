using System.Collections.Generic;
using UnityEngine;

namespace Pretia.RelocChecker.Plateau
{
    [RequireComponent(typeof(MeshRenderer))]
    public class TransparencyController : MonoBehaviour
    {
        private const string TRANSPARENCY_PROPERTY_NAME = "_Transparency";
        private static readonly int TransparencyProperty = Shader.PropertyToID(TRANSPARENCY_PROPERTY_NAME);
        
        private static readonly List<TransparencyController> ActiveComponents = new();

        public static System.Action OnElementAdded;
        
        [SerializeField]
        private Material material;
        
#if UNITY_EDITOR
        private void Reset()
        {
            var meshRenderer = GetComponent<MeshRenderer>();
            var mainMaterial = meshRenderer.sharedMaterial;
            if (!mainMaterial.HasFloat(TransparencyProperty))
            {
                Debug.LogError($"The game object \"{gameObject}\" does not have a Transparency property.");
                return;
            }

            material = mainMaterial;
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
                if (component.material == null || !component.material.HasFloat(TransparencyProperty))
                {
                    continue;
                }
                
                component.material.SetFloat(TransparencyProperty, transparency);
            }
        }
    }
}