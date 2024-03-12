using System.Collections.Generic;
using Scripts.Utils;
using UnityEditor;
using UnityEngine;

namespace Scripts
{
    public class CheckCircleOverlaps : MonoBehaviour
    {
        [SerializeField] private float _radius = 1f;  
        private readonly Collider2D[] _interactionResult = new Collider2D[10];

        public GameObject[] GetObjectsInRange()
        {
        var size = Physics2D.OverlapCircleNonAlloc(
          transform.position,
         _radius,
          _interactionResult);
            

        var overlaps = new List<GameObject>();  
        for (var i = 0; i < size ; i++)
            {
                overlaps.Add(_interactionResult[i].gameObject);                                
                Debug.Log($"Detected object: {_interactionResult[i].gameObject.name} at position {_interactionResult[i].gameObject.transform.position}");
            }
            return overlaps.ToArray();
        }
        private void OnDrawGizmosSelected()
        {
            Handles.color = HandlesUtils.TransparentRed;
            Handles.DrawSolidDisc(transform.position, Vector3.forward, _radius);
        }
    }

}
