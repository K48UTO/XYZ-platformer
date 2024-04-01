using System;
using System.Collections.Generic;
using System.Linq;
using Scripts.Utils;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Scripts
{
    public class CheckCircleOverlaps : MonoBehaviour
    {
        [SerializeField] private float _radius = 1f;
        [SerializeField] private LayerMask _mask;
        [SerializeField] private string[] _tags;
        [SerializeField] private OnOvelapEvent _onOverLap;

        
        private readonly Collider2D[] _interactionResult = new Collider2D[10];

    
        private void OnDrawGizmosSelected()
        {
            Handles.color = HandlesUtils.TransparentRed;
            Handles.DrawSolidDisc(transform.position, Vector3.forward, _radius);
        }

        public void Check()
        {
            var size = Physics2D.OverlapCircleNonAlloc(
                transform.position,
                _radius,
                _interactionResult,
                _mask);

            for (var i = 0; i < size; i++)
            {
                var overlapResult = _interactionResult[i];
                var isInTag = _tags.Any(tag => overlapResult.CompareTag(tag));
                if (isInTag)
                {
                    _onOverLap?.Invoke(_interactionResult[i].gameObject);

                }
            }
        }

        [Serializable]
        public class OnOvelapEvent : UnityEvent <GameObject> 
        {
        }

    }

}
