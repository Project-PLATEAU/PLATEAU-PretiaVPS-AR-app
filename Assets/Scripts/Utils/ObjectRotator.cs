using UnityEngine;

namespace Pretia.RelocChecker.Utils
{
    public class ObjectRotator : MonoBehaviour
    {
        public GameObject childObject;
        public float rotationSpeed = 50.0f;
        public bool useLocalRotation = true;

        void Update()
        {
            RotateObject();
        }

        private void RotateObject()
        {
            if (childObject != null)
            {
                float rotationAmount = rotationSpeed * Time.deltaTime;

                if (useLocalRotation)
                {
                    childObject.transform.Rotate(Vector3.up, rotationAmount, Space.Self);
                }
                else
                {
                    childObject.transform.Rotate(Vector3.up, rotationAmount, Space.World);
                }
            }
        }
    }
}