using System;
using Scripts.Utils;
using UnityEngine;
using UnityEngine.Events;



namespace Scripts.Components.ColliderBased
{
    public class EnterTriggerComponent : MonoBehaviour
    {
        [SerializeField] private string _tag;
        [SerializeField] private string _exitTag;

        [SerializeField] private LayerMask _layer = ~0;

        [SerializeField] private EnterEvent _enterAction;
        [SerializeField] private EnterEvent _exitAction;


        private void OnTriggerEnter2D(Collider2D other)

        {
            if (!other.gameObject.IsInLayer(_layer)) return;

            if (!string.IsNullOrEmpty(_tag) && !other.gameObject.CompareTag(_tag)) return;
            _enterAction?.Invoke(other.gameObject);
           
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (string.IsNullOrEmpty(_exitTag) || other.gameObject.CompareTag(_exitTag))
            {

                _exitAction?.Invoke(other.gameObject);
            }
        }

        [Serializable]
        public class EnterEvent : UnityEvent<GameObject>
        {

        }
    }
}

