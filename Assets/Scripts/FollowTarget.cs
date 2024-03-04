
using UnityEngine;



namespace Scripts
{
    public class FollowTarget : MonoBehaviour
    {

        [SerializeField] private Transform _target;
        [SerializeField] private float smoothing = 5f;
        [SerializeField]  Vector3 offset;

        private void LateUpdate()
        {
            Vector3 targetCamPos = _target.position+offset;
       
            transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
        }
    }
}

