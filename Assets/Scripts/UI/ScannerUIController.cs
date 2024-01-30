using System.Collections.Generic;
using Pretia.RelocChecker.Plateau;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Pretia.RelocChecker.UI
{
    public class ScannerUIController : MonoBehaviour
    {   
        [FormerlySerializedAs("_relocController")] [SerializeField]
        RelocalizationController relocController;

        [SerializeField]
        private List<GameObject> idleUI;
        
        [FormerlySerializedAs("_mapScanningUI")] [SerializeField]
        private List<GameObject> mapScanningUI;

        [FormerlySerializedAs("_mapRelocalizedUI")] [SerializeField]
        private List<GameObject> mapRelocalizedUI;
       
        [SerializeField]
        private List<GameObject> initFailsUI;

        [FormerlySerializedAs("_mapNameText")] [SerializeField]
        private TMP_Text mapNameText;
        
        [SerializeField]
        private GameObject mapNameObject;

        [FormerlySerializedAs("_playButton")] [SerializeField]
        private Button playButton;

        [FormerlySerializedAs("_stopButton")] [SerializeField]
        private Button stopButton;

        public void Awake()
        {
            relocController.OnMapRelocalized += OnMapRelocalized;
            relocController.OnMapNameFound += OnNameChanged;
            relocController.OnInitializationException += OnInitializationException;
            relocController.OnReset += OnReset;
            relocController.OnStopNotInitialized += OnStop;
            
            playButton.onClick.AddListener(Play);
            stopButton.onClick.AddListener(Stop);
            
            DisplayUI(idleUI);
            playButton.gameObject.SetActive(true);
        }

        public void OnDestroy()
        {
            relocController.OnMapRelocalized -= OnMapRelocalized;
            relocController.OnMapNameFound -= OnNameChanged;
            relocController.OnInitializationException -= OnInitializationException;
            relocController.OnReset -= OnReset;
            relocController.OnStopNotInitialized -= OnStop;
            playButton.onClick.RemoveListener(Play);
            stopButton.onClick.RemoveListener(Stop);
        }

        private void Play()
        {
            relocController.StartReloc();
            playButton.gameObject.SetActive(false);
            stopButton.gameObject.SetActive(true);
            SetUI(initFailsUI, false);
            DisplayUI(mapScanningUI);
        }
        
        private void Stop()
        {
            relocController.StopReloc();
            OnStop();
        }

        private void OnStop()
        {
            DisplayUI(idleUI);
            playButton.gameObject.SetActive(true);
            stopButton.gameObject.SetActive(false);
        }
        
        private void OnInitializationException()
        {
            playButton.gameObject.SetActive(true);
            stopButton.gameObject.SetActive(false);
            SetUI(initFailsUI, true);
        }

        private void OnNameChanged(string mapName)
        {
            mapNameObject.SetActive(!string.IsNullOrEmpty(mapName));
            mapNameText.text = mapName != RelocalizationController.DEFAULT_MAP_NAME ? mapName : "未知の名前";
        }
        
        private void OnReset()
        {
            mapNameObject.SetActive(false);
        }

        private void OnMapRelocalized()
        {
            DisplayUI(mapRelocalizedUI);
            stopButton.gameObject.SetActive(false);
            playButton.gameObject.SetActive(false);
            SetUI(initFailsUI, false);
        }

        private void DisplayUI(List<GameObject> list)
        {
            Clear();
            SetUI(list, true);
        }

        private void Clear()
        {
            SetUI(mapScanningUI,false);
            SetUI(mapRelocalizedUI,false);
            SetUI(idleUI,false);
        }

        private void SetUI(List<GameObject> uiList, bool active)
        {
            foreach (var uiElement in uiList)
                uiElement.SetActive(active);
        }
    }
}
