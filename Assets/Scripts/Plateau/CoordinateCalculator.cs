using System.Collections.Generic;
using Pretia.RelocChecker.Plateau.Bridging;
using UnityEngine;

namespace Pretia.RelocChecker.Plateau
{
    public class CoordinateCalculator : MonoBehaviour
    {
        private PlateauGeoCoordinateComponent Origin { get; set; }
        private List<PlateauGeoCoordinateComponent> KnownCoordinatesList { get; } = new();
        public System.Action<bool, double, double> OnCoordinatesChanged { get; set; }
        
        private void Awake()
        {
            Origin = null;
            KnownCoordinatesList.Clear();
        }

        private void OnEnable()
        {
            PlateauGeoCoordinateComponent.OnComponentAdded += OnComponentAdded;
            PlateauGeoCoordinateComponent.OnComponentRemoved += OnComponentRemoved;
        }
        
        private void Update()
        {
            if (Origin)
            {
                Evaluate();
            }
            else
            {
                OnCoordinatesChanged?.Invoke(false, 0, 0);
            }
        }

        private void OnDisable()
        {
            PlateauGeoCoordinateComponent.OnComponentAdded -= OnComponentAdded;
            PlateauGeoCoordinateComponent.OnComponentRemoved -= OnComponentRemoved;
        }

        private void OnDestroy()
        {
            Origin = null;
            KnownCoordinatesList.Clear();
        }

        private void OnComponentAdded(PlateauGeoCoordinateComponent obj)
        {
            if (KnownCoordinatesList.Contains(obj))
            {
                return;
            }

            if (Origin == null)
            {
                Origin = obj;
            }
            
            KnownCoordinatesList.Add(obj);
        }

        private void OnComponentRemoved(PlateauGeoCoordinateComponent obj)
        {
            if (obj == Origin && KnownCoordinatesList.Exists(o => o != obj))
            {
                Origin = KnownCoordinatesList.Find(o => o != obj);
            }
            
            if (!KnownCoordinatesList.Contains(obj))
            {
                return;
            }

            KnownCoordinatesList.Remove(obj);
        }

        private void Evaluate()
        {

            if (!Origin)
            {
                return;
            }

            var Lon0 = Origin.Longitude;
            var Lat0 = Origin.Latitude;
            
            PlaneXYConv.LonLat2XY(Lon0, Lat0, Lon0, Lat0, out var x0, out var y0);
            
            Debug.Log($"Pos0 = {x0},{y0}");
            
            var pos0 = Vector3.ProjectOnPlane(Origin.transform.position, Vector3.up);
            var pos = Vector3.ProjectOnPlane(transform.position, Vector3.up);
            var diff = pos - pos0;
            
            Debug.Log($"Diff{diff}");

            var x = x0 + diff.x;
            var y = y0 + diff.z;
            
            Debug.Log($"Pos = {x},{y}");
            
            PlaneXYConv.XY2LonLat(x,y, Lon0, Lat0, out var lon, out var lat);
            
            OnCoordinatesChanged?.Invoke(true, lon , lat);
        }
        
    }
}