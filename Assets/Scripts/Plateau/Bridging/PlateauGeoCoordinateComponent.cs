#if PLATEAU_SDK_2_2_0_ALPHA
using PLATEAU.CityInfo;
using PLATEAU.Native;
#endif
using UnityEngine;

namespace Pretia.RelocChecker.Plateau.Bridging
{

    [ExecuteInEditMode]
    public class PlateauGeoCoordinateComponent : MonoBehaviour
    {
        public static System.Action<PlateauGeoCoordinateComponent> OnComponentAdded;
        public static System.Action<PlateauGeoCoordinateComponent> OnComponentRemoved;
        public static System.Action<PlateauGeoCoordinateComponent> OnVisible;
        public static System.Action<PlateauGeoCoordinateComponent> OnInvisible;

        [SerializeField, HideInInspector] private float latitude;

        [SerializeField, HideInInspector] private float longitude;

        public float Latitude => latitude;

        public float Longitude => longitude;

#if PLATEAU_SDK_2_2_0_ALPHA
        [SerializeField] 
        private PLATEAUInstancedCityModel cityModel;
#endif

#if UNITY_EDITOR
        [SerializeField] private string geoCoordinateText;
#endif


#if PLATEAU_SDK_2_2_0_ALPHA && UNITY_EDITOR
        private void Reset()
        {
            cityModel = GetComponentInParent<PLATEAUInstancedCityModel>();
            EvaluateGeoCoordinate();
        }

        private void Update()
        {
            if(transform.hasChanged)
                EvaluateGeoCoordinate();
        }

        private void EvaluateGeoCoordinate()
        {
            if (!cityModel)
            {
                return;
            }
            
            var diff = transform.position - cityModel.transform.position;
            var geoCoordinate = cityModel.GeoReference.Unproject(new PlateauVector3d(diff.x, diff.y, diff.z));
            
            latitude = (float) geoCoordinate.Latitude;
            longitude = (float)geoCoordinate.Longitude;
            geoCoordinateText = $"{latitude}, {longitude}";
        }
#endif

        private void OnEnable()
        {
#if PLATEAU_SDK_2_2_0_ALPHA && UNITY_EDITOR
            EvaluateGeoCoordinate();
#endif
            if (Application.isPlaying)
            {
                OnComponentAdded?.Invoke(this);
            }
        }

        private void OnDisable()
        {
            if (Application.isPlaying)
            {
                OnComponentRemoved?.Invoke(this);
            }
        }

        private void OnBecameVisible()
        {
            if (Application.isPlaying)
            {
                OnVisible?.Invoke(this);
            }
        }

        private void OnBecameInvisible()
        {
            if (Application.isPlaying)
            {
                OnInvisible?.Invoke(this);
            }
        }
    }
}