using UnityEngine;

namespace Pretia.RelocChecker.Utils
{
    public class LinealAnimationController : MonoBehaviour
    {
        [SerializeField] private new AnimationCurve animation;

        [SerializeField] private Transform startPoint;

        [SerializeField] private Transform endPoint;

        [SerializeField] private float speed = 1;

        private float Transition { get; set; }
        private bool ShouldDecrease { get; set; }

        private void Update()
        {
            transform.position = Vector3.Lerp(startPoint.position, endPoint.position, animation.Evaluate(Transition));

            if (ShouldDecrease)
            {
                Transition -= Time.deltaTime * speed;
                ShouldDecrease = Transition > 0;
            }
            else
            {
                Transition += Time.deltaTime * speed;
                ShouldDecrease = Transition > 1;
            }
        }
    }
}
