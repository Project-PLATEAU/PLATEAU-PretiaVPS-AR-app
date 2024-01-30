using UnityEngine;

namespace Pretia.RelocChecker.Utils
{
    public class LookAtCamera : MonoBehaviour
    {
        private static Camera MainCamera { get; set; }
        private static bool HasCamera => MainCamera != null;
        
        private static void FindCamera()
        {
            MainCamera = Camera.main;
            if (MainCamera == null)
            {
                MainCamera = FindObjectOfType<Camera>();
            }
        }
        
        private void OnEnable()
        {
            if (!HasCamera)
            {
                FindCamera();
            }
        }

        private void Update()
        {
            if (!HasCamera)
            {
                return;
            }
            
            transform.LookAt(MainCamera.transform);
        }
    }
}