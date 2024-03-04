using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



namespace Scripts.Components
{    public class EnterTriggerComponent : MonoBehaviour
    {
        [SerializeField] private string _tag;
        [SerializeField] private EnterEvent _action;
        [SerializeField] private EnterEvent _exitAction;


        private void OnTriggerEnter2D(Collider2D other)

        {
            
            if(other.gameObject.CompareTag(_tag))
            {
                _action?.Invoke(other.gameObject);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.CompareTag(_tag))
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

