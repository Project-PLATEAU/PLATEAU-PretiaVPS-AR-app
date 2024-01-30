using System.Collections;
using PretiaArCloud;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;

namespace Pretia.RelocChecker.Plateau
{
    public class PlateauContentAligner : MonoBehaviour
    {
        [SerializeField] 
        private ARSharedAnchorManager relocManager;

        [SerializeField]
        private Transform rootContent;

        [SerializeField] 
        private Transform rootPointCloud;

        [SerializeField] 
        private UnityEvent<string> logEvent;

        public System.Action<bool> OnAlignmentInfoFound { get; set; }
        public System.Action<bool> OnAlignmentInfoExperimentalFound { get; set; }
        
        public System.Action<string> ContentStatusChanged { get; set; }

        private float[][] AlignmentMatrix { get; set; }
        private float[][] AlignmentMatrixExperimental { get; set; }

        private bool UseExperimental { get; set; }
        private bool DidRelocalize { get; set; }

        private void OnEnable()
        {
            relocManager.OnMapRelocalizationStarted += OnMapRelocalizationStarted;
            relocManager.OnAlignmentChanged += OnAlignmentChanged;
            relocManager.OnRelocalized += OnRelocalized;
            DidRelocalize = false;
            
            ContentStatusChanged?.Invoke(string.Empty);
        }

        private void Start()
        {
            logEvent.AddListener(LogMessage);
        }

        private void OnDisable()
        {
            relocManager.OnMapRelocalizationStarted -= OnMapRelocalizationStarted;
            relocManager.OnAlignmentChanged -= OnAlignmentChanged;
            relocManager.OnRelocalized -= OnRelocalized;
        }

        private void OnMapRelocalizationStarted(string mapKey)
        {
            DidRelocalize = false;
            StartCoroutine(LoadAsset(mapKey));
        }

        private IEnumerator LoadAsset(string mapKey)
        {
            ContentStatusChanged?.Invoke("Updating Catalog...");
            // yield return Addressables.LoadContentCatalogAsync(PlateauConstants.REMOTE_CATALOG_PATH);

            var loadAsset = Addressables.LoadAssetAsync<GameObject>(mapKey);

            while (!loadAsset.IsDone)
            {
                var progress = loadAsset.PercentComplete;
                ContentStatusChanged?.Invoke($"Downloading: {progress * 100}%");
                yield return null;
            }

            if (loadAsset.Result == null)
            {
                logEvent.Invoke($"Content not downloaded for {mapKey}");
                yield break;
            }
    
            Instantiate(loadAsset.Result, rootContent);
            logEvent.Invoke($"Content downloaded for {mapKey}");
            
            loadAsset = Addressables.LoadAssetAsync<GameObject>(mapKey+"_pc");
            
            while (!loadAsset.IsDone)
            {
                var progress = loadAsset.PercentComplete;
                ContentStatusChanged?.Invoke($"Downloading PC: {progress * 100}%");
                yield return null;
            }
            
            ContentStatusChanged?.Invoke(string.Empty);
            
            if (loadAsset.Result == null)
            {
                logEvent.Invoke($"PC not downloaded for {mapKey}");
                yield break;
            }
            
            Instantiate(loadAsset.Result, rootPointCloud);
            logEvent.Invoke($"PC downloaded for {mapKey}");
        }

        private void OnAlignmentChanged(PlateauAlignInfo info)
        {
            if (info == null)
            {
                OnAlignmentInfoFound?.Invoke(false);
                return;
            }
            
            AlignmentMatrix = info.AlignmentMatrix;
            OnAlignmentInfoFound?.Invoke(AlignmentMatrix != null);
            
            AlignmentMatrixExperimental = info.AlignmentMatrixExperimental;
            OnAlignmentInfoExperimentalFound?.Invoke(AlignmentMatrixExperimental != null);

            if (info.AlignmentMatrix != null)
            {
                logEvent.Invoke($"Alignment Matrix : {relocManager.GetMatrix(info.AlignmentMatrix)}");
            }
            if (info.AlignmentMatrixExperimental != null)
            {
                logEvent.Invoke($"Alignment Matrix NN : {relocManager.GetMatrix(info.AlignmentMatrixExperimental)}");
            }
        }
        
        private void OnRelocalized()
        {
            DidRelocalize = true;
            Align();
        }

        public void SetAlignmentMatrix(bool useExperimental)
        {
            UseExperimental = useExperimental;
            if (DidRelocalize)
            {
                Align();
            }
        }

        private void Align()
        {
            // Reset root.
            rootContent.localPosition = Vector3.zero;
            rootContent.localEulerAngles = Vector3.zero;
            rootContent.localScale = Vector3.one;
            
            if (UseExperimental && AlignmentMatrixExperimental != null)
            {
                relocManager.Align(rootContent, AlignmentMatrixExperimental);
                rootContent.gameObject.SetActive(true);
            }
            else if(AlignmentMatrix != null)
            {
                relocManager.Align(rootContent, AlignmentMatrix);
                rootContent.gameObject.SetActive(true);
            }
            else
            {
                Debug.LogError("No alignment matrix found.");
            }
            Debug.Log($"Content: {rootContent.position}, {rootContent.eulerAngles}, {rootContent.localScale}");
        }

        private void LogMessage(string message)
        {
            Debug.Log(message);
        }
    }
}
