using System.Collections.Generic;
using Pretia.RelocChecker.Plateau;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

namespace Pretia.RelocChecker.UI
{
    
    public class MapSelectionDropDown : MonoBehaviour
    {
        [SerializeField]
        private AssetReference mapCollection;
        
        [SerializeField]
        private CustomMapSelection buildInMapSelection;

        [SerializeField]
        private TMP_InputField inputText;
        
        [SerializeField]
        private TMP_Dropdown dropdown;
        
        private CustomMapSelection _mapSelection;

        private readonly List<string> _options = new();

#if UNITY_EDITOR
        private void Reset()
        {
            if (dropdown == null)
            {
                dropdown = GetComponent<TMP_Dropdown>();
            }
        }
#endif

        private async void Awake()
        {
            var remoteMapSelection = await CustomMapSelection.LoadCustomMapSelection(mapCollection);
            _mapSelection = buildInMapSelection;
            _mapSelection.MergeDirectMapsSelection(remoteMapSelection);
            
            ResetDropdownOption();
            
            dropdown.onValueChanged.AddListener(OnValueChanged);
            inputText.onSubmit.AddListener(OnSubmit);
            inputText.onEndEdit.AddListener(OnEndEdit);
        }

        private void OnDestroy()
        {
            inputText.onSubmit.RemoveListener(OnSubmit);
            dropdown.onValueChanged.RemoveListener(OnValueChanged);
            inputText.onEndEdit.RemoveListener(OnEndEdit);
        }

        private void OnValueChanged(int selection)
        {

            if (selection < 0)
            {
                _mapSelection.SetToCriteriaBased();
            }

            else if (selection >= _options.Count -1)
            {
                EnableInputField();
            }

            else
            {
                _mapSelection.SetMapKeySelection(selection - 1);
            }
        }

        private void ResetDropdownOption()
        {
            dropdown.ClearOptions();
            
            _options.Clear();
            _options.Add("Criteria Based Selection");
            
            var mapNames = _mapSelection.GetAvailableMaps();
            for (var i = 0; i < mapNames.Length; i++)
            {
                _options.Add(mapNames[i]);
            }
            
            _options.Add("Custom Selection");
            
            dropdown.AddOptions(_options);
            
            _mapSelection.SetToCriteriaBased();
            
            LayoutRebuilder.ForceRebuildLayoutImmediate(dropdown.transform as RectTransform);
        }

        private void EnableInputField()
        {
            inputText.gameObject.SetActive(true);
            inputText.Select();
        }

        private void OnEndEdit(string input)
        {
            if (!string.IsNullOrEmpty(input))
            {
                return;
            }
            
            dropdown.value = 0;
            inputText.gameObject.SetActive(false);
            dropdown.Select();
        }

        private void OnSubmit(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return;
            }
            
            var index = _mapSelection.AddMapKey(input);
            ResetDropdownOption();
            dropdown.value = index + 1;
        }
    }
}