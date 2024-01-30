using UnityEngine;
using PretiaArCloud;
using System.Threading;
using System;
using UnityEngine.Serialization;

namespace Pretia.RelocChecker.Plateau
{
    public class RelocalizationController : MonoBehaviour
    {
        [FormerlySerializedAs("_arSharedAnchorManager")] 
        [SerializeField]
        private ARSharedAnchorManager arSharedAnchorManager;

        [SerializeField]
        private MapCollection mapCollection;

        public const string DEFAULT_MAP_NAME = "UNKNOWN";

        public Action OnMapRelocalized { get; set; }
        public Action OnInitializationException { get; set; }
        public Action<string> OnMapNameFound { get; set; }
        public Action OnReset { get; set; }
        public Action OnStopNotInitialized { get; set; }

        private bool DidInitialize { get; set; }
        private bool DidRelocalize { get; set; }
        
        private void Awake()
        {
            Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;

            arSharedAnchorManager.OnException += OnRelocException;
            arSharedAnchorManager.OnMapRelocalizationStarted += OnMapKeyFound;
            arSharedAnchorManager.OnRelocalized += OnRelocalized;
            arSharedAnchorManager.OnRelocalizationStateChanged += OnRelocalizationStateChanged;
        }

        private void OnDestroy()
        {
            StopReloc();
            arSharedAnchorManager.OnException -= OnRelocException;
            arSharedAnchorManager.OnMapRelocalizationStarted -= OnMapKeyFound;
            arSharedAnchorManager.OnRelocalized -= OnRelocalized;
            arSharedAnchorManager.OnRelocalizationStateChanged -= OnRelocalizationStateChanged;
        }

        private void OnRelocalizationStateChanged(RelocalizationState state)
        {
            if (state == RelocalizationState.Stopped && !DidRelocalize)
            {
                if (!DidInitialize)
                {
                    OnMapNameFound?.Invoke(string.Empty);
                    OnInitializationException?.Invoke();
                    StopReloc();
                    OnStopNotInitialized?.Invoke();
                }
                else
                {
                    ResetReloc();
                }
            }

            if (state == RelocalizationState.Relocalizing && !DidInitialize)
            {
                DidInitialize = true;
            }
            Debug.Log($"Relocalization State: {state}");
        }

        private void OnMapKeyFound(string mapKey)
        {
            var hasValue = mapCollection.TryFoundMap(mapKey, out var found);
            OnMapNameFound?.Invoke(hasValue ? found.name: DEFAULT_MAP_NAME);
            Debug.Log($"Map Key: {mapKey}");
        }

        private void OnRelocalized()
        {
            DidRelocalize = true;
            OnMapRelocalized?.Invoke();
            StopReloc();
        }

        private void OnRelocException(Exception e)
        {
            Debug.LogError(e, arSharedAnchorManager);
        }

        public void StartReloc()
        {
            if (arSharedAnchorManager.RelocalizationState != RelocalizationState.Stopped)
            {
                return;
            }

            if (DidInitialize)
            {
                ResetReloc();
                return;
            }
            
            arSharedAnchorManager.StartCloudMapRelocalization();
            Debug.Log($"Relocalization Start");
        }
        
        public void StopReloc()
        {
            arSharedAnchorManager.StopRelocalization();
            arSharedAnchorManager.ResetSharedAnchor();
            Debug.Log($"Relocalization Stop");
        }

        private void ResetReloc()
        {
            StopReloc();
            DidInitialize = false;
            DidRelocalize = false;
            OnReset?.Invoke();
            StartReloc();
        }
    }
}

