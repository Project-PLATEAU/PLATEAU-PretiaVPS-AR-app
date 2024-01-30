using System.Collections.Generic;
using Pretia.RelocChecker.Plateau.Bridging;
using UnityEngine;

namespace Pretia.RelocChecker.UI
{
    public class PlateauCoordinatesController : MonoBehaviour
    {
        [SerializeField]
        private GeoLocationDisplay prefab;

        [SerializeField] 
        private Transform container;

        [SerializeField] 
        private Camera cam;
        
        private Dictionary<PlateauGeoCoordinateComponent,GeoLocationDisplay> Displays { get; } = new();
        
        private void Awake()
        {
            PlateauGeoCoordinateComponent.OnComponentAdded += OnComponentAdded;
            PlateauGeoCoordinateComponent.OnVisible += OnVisible;
            PlateauGeoCoordinateComponent.OnInvisible += OnInvisible;
            PlateauGeoCoordinateComponent.OnComponentRemoved += OnComponentRemoved;
        }

        private void OnDestroy()
        {
            PlateauGeoCoordinateComponent.OnComponentAdded -= OnComponentAdded;
            PlateauGeoCoordinateComponent.OnVisible -= OnVisible;
            PlateauGeoCoordinateComponent.OnInvisible -= OnInvisible;
            PlateauGeoCoordinateComponent.OnComponentRemoved -= OnComponentRemoved;
        }

        private void OnComponentAdded(PlateauGeoCoordinateComponent component)
        {
            if (Displays.ContainsKey(component))
            {
                return;
            }

            var display = Instantiate(prefab, container, worldPositionStays: false);
            display.SetTarget(component);
            display.SetCurrentCamera(cam);
            display.gameObject.SetActive(false);
            
            Displays.Add(component, display);
        }

        private void OnComponentRemoved(PlateauGeoCoordinateComponent component)
        {
            if (!Displays.ContainsKey(component))
            {
                return;
            }
            
            Destroy(Displays[component].gameObject);
        }

        private void OnVisible(PlateauGeoCoordinateComponent component)
        {
            if (!Displays.ContainsKey(component))
            {
                return;
            }
            
            Displays[component].gameObject.SetActive(true);
        }

        private void OnInvisible(PlateauGeoCoordinateComponent component)
        {
            if (!Displays.ContainsKey(component))
            {
                return;
            }
            
            Displays[component].gameObject.SetActive(false);
        }
    }
}