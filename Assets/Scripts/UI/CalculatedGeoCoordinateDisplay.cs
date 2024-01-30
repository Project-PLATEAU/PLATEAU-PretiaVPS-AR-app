using Pretia.RelocChecker.Plateau;
using TMPro;
using UnityEngine;

namespace Pretia.RelocChecker.UI
{
    public class CalculatedGeoCoordinateDisplay : MonoBehaviour
    {
        [SerializeField] 
        private TMP_Text label;
        
        [SerializeField] 
        private CoordinateCalculator calculator;

        private void OnEnable()
        {
            calculator.OnCoordinatesChanged += OnCoordinatesChanged;
        }

        private void OnCoordinatesChanged(bool found, double latitude, double longitude)
        {
            label.gameObject.SetActive(found);
            label.text = "Plateau: " + CoordinatesFormatter.FormatCoordinates((float)longitude, (float) latitude);
        }
    }
}