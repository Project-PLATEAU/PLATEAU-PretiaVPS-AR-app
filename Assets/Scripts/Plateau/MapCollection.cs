using System.Collections.Generic;
using UnityEngine;

namespace Pretia.RelocChecker.Plateau
{
    [CreateAssetMenu(fileName = "MapCollection", menuName = "Plateau/MapCollection", order = 0)]
    public class MapCollection : ScriptableObject
    {
        [SerializeField]
        internal List<MapInfo> maps;

        public bool TryFoundMap(string mapKey, out MapInfo mapInfoFound)
        {
            mapInfoFound = maps.Find(mapInfo => mapInfo.mapKey == mapKey);
            return mapInfoFound != null;
        }
    }
}