using System;
using Scripts.Creatures;
using UnityEngine;
using UnityEngine.Events;

namespace Scripts.Components.ColliderBased
{
    public class EnterCollisionComponent : MonoBehaviour
    {
        [SerializeField] private string _tag; 
        [SerializeField] private EnterEvent _action; 
        [SerializeField] private EnterEvent _exitAction;

        private void OnCollisionEnter2D(Collision2D other)
        {
           
            if (!string.IsNullOrEmpty(_tag))
            {
              
                if (other.gameObject.CompareTag(_tag))
                {
                    _action?.Invoke(other.gameObject);
                }
            }
            else
            {
          
                if (other.gameObject.GetComponent<Hero>() != null)
                {
                    _action?.Invoke(other.gameObject);
                }
            }
        }

        private void OnCollisionExit2D(Collision2D other)
        {
     
            if (!string.IsNullOrEmpty(_tag))
            {
                if (other.gameObject.CompareTag(_tag))
                {
                    _exitAction?.Invoke(other.gameObject);
                }
            }
            else
            {
                if (other.gameObject.GetComponent<Hero>() != null)
                {
                    _exitAction?.Invoke(other.gameObject);
                }
            }
        }

        [Serializable]
        public class EnterEvent : UnityEvent<GameObject>
        {
        }
    }
}
