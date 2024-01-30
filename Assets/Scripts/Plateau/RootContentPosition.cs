using UnityEngine;

namespace Pretia.RelocChecker.Plateau
{
    [CreateAssetMenu(fileName = "RootContentRemote", menuName = "Plateau/RootContent", order = 0)]
    public class RootContentPosition : ScriptableObject
    {
        [SerializeField] private Vector3 position;
        [SerializeField] private Vector3 rotation;
        [SerializeField] private Vector3 scale;

        public Vector3 Position => position;
        public Vector3 Rotation => rotation;
        public Vector3 Scale => scale;
    }
}