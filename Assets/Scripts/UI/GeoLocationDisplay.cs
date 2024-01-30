using Pretia.RelocChecker.Plateau;
using Pretia.RelocChecker.Plateau.Bridging;
using TMPro;
using UnityEngine;

namespace Pretia.RelocChecker.UI
{
    public class GeoLocationDisplay : MonoBehaviour
    {
        [SerializeField] 
        private TMP_Text label;
        
        private Camera Camera { get; set; }

        private PlateauGeoCoordinateComponent Target { get; set; }

        private void SetLabel(float longitude, float latitude)
        {
            label.text = label.text = CoordinatesFormatter.FormatCoordinates(latitude, longitude);
        }

        public void SetTarget(PlateauGeoCoordinateComponent target)
        {
            Target = target;
            if (!Target)
            {
                return;
            }
            
            SetLabel(target.Longitude, target.Latitude);
        }

        public void SetCurrentCamera(Camera cam)
        {
            Camera = cam;
        }

        private void Update()
        {
            if (!Target)
            {
                return;
            }
            
            // Check if the object is visible
            if (Target.gameObject.activeInHierarchy)
            {
                // Convert the object's position from world space to screen space
                Vector2 screenPosition = Camera.WorldToScreenPoint(Target.transform.position);
            
                // Update the UI element's position
                transform.position = screenPosition;
            }
        }
    }
}