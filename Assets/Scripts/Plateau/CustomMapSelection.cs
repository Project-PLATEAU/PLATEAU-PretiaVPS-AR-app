using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PretiaArCloud;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Pretia.RelocChecker.Plateau
{
    [CreateAssetMenu(fileName = "CustomPlateauMapSelection", menuName = "Pretia ArCloud/Map Selection/Custom Plateau Map Selection")]
    public class CustomMapSelection : AbstractMapSelection
    {
        private enum SupportedSelection
        {
            CriteriaBased,
            DirectMap
        }

        [SerializeField]
        private SupportedSelection selection;
        
        [SerializeField]
        private MapSelectionCriteria criteria;

        private List<MapInfo> Maps { get; } = new(); 
        private MapInfo SelectedMapInfo { get; set; }
        public override bool RequiresLocation => selection == SupportedSelection.CriteriaBased;

        public override async Task<string> GetMapSelectionAsync(CancellationToken cancellationToken = default)
        {
            switch (selection)
            {
                case SupportedSelection.CriteriaBased:
                    await LocationProvider.StartLocationServiceAsync((int)LocationId.MapSelection, cancellationToken);
                    var selections = await MapSelection.SelectMapsAsync(criteria, cancellationToken);
                    return selections == null 
                        ? string.Empty 
                        : selections.SelectedMaps.Select(m => m.MapKey).FirstOrDefault();
                case SupportedSelection.DirectMap:
                    return SelectedMapInfo != null ? SelectedMapInfo.mapKey : string.Empty;
                default:
                    return string.Empty;
            }
        }

        public void MergeDirectMapsSelection(MapCollection selectionToMerge)
        {
            if (!selectionToMerge || selectionToMerge.maps == null)
            {
                return;
            }
            
            for (var i = 0; i < selectionToMerge.maps.Count; i++)
            {
                var map = selectionToMerge.maps[i];
                if (Maps.Exists(mapInList => mapInList.mapKey == map.mapKey && mapInList.name == map.name))
                {
                    continue;
                }
                Maps.Add(map);
            }
        }

        public void SetMapKeySelection(int index)
        {
            if (index < 0 || index >= Maps.Count) return;
            SelectedMapInfo = Maps[index];
            selection = SupportedSelection.DirectMap;
        }

        public void SetToCriteriaBased()
        {
            SelectedMapInfo = null;
            selection = SupportedSelection.CriteriaBased;
        }

        public int AddMapKey(string mapKey)
        {
            Maps.Add(new MapInfo(){mapKey = mapKey, name = mapKey});
            return Maps.Count - 1;
        }

        public string[] GetAvailableMaps()
        {
            var names = new string[Maps.Count];
            for (var i = 0; i < Maps.Count(); i++)
            {
                names[i] = Maps[i].name;
            }

            return names;
        }

        public static async Task<MapCollection> LoadCustomMapSelection(AssetReference reference)
        {
            //await Addressables.LoadContentCatalogAsync(...).Task;
            var asset = await reference.LoadAssetAsync<MapCollection>().Task;
            return asset;
        }
    }
}