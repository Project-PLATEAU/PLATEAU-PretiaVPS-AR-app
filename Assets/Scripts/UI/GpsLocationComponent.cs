using System.Collections;
using Pretia.RelocChecker.Plateau;
using UnityEngine;
using TMPro;

namespace Pretia.RelocChecker.UI
{
    public class GpsLocationComponent : MonoBehaviour
    {
        [SerializeField] private TMP_Text label; // Reference to the TextMeshPro text component

        private void Start()
        {
            if (!Input.location.isEnabledByUser)
            {
                label.text = "Location services not enabled by user";
                return;
            }

            Input.location.Start();

            StartCoroutine(InitLocationService());
        }

        private IEnumerator InitLocationService()
        {
            var maxWait = 20;
            while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
            {
                yield return new WaitForSeconds(1);
                maxWait--;
            }

            if (maxWait < 1)
            {
                label.text = "Location service initialization timed out";
                yield break;
            }

            if (Input.location.status == LocationServiceStatus.Failed)
            {
                label.text = "Unable to determine device location";
                yield break;
            }

            label.text = CoordinatesFormatter.FormatCoordinates(
                Input.location.lastData.latitude,
                Input.location.lastData.longitude
            );
        }

        private void Update()
        {
            if (Input.location.status == LocationServiceStatus.Running)
            {
                label.text = "GPS: " + CoordinatesFormatter.FormatCoordinates(
                    Input.location.lastData.latitude,
                    Input.location.lastData.longitude
                );
            }
        }
    }
}