using System;
using Scripts.Creatures;
using UnityEngine;
using UnityEngine.Events;

namespace Scripts.Components.ColliderBased
{
    public class EnterCollisionComponent : MonoBehaviour
    {
        [SerializeField] private string _tag; // Оставляем возможность задавать тег
        [SerializeField] private EnterEvent _action;
        [SerializeField] private EnterEvent _exitAction;

        private void OnCollisionEnter2D(Collision2D other)
        {
            // Сначала проверяем, указан ли тег для проверки
            if (!string.IsNullOrEmpty(_tag))
            {
                // Если тег указан, то используем его для проверки
                if (other.gameObject.CompareTag(_tag))
                {
                    _action?.Invoke(other.gameObject);
                }
            }
            else
            {
                // Если тег не указан, проверяем наличие компонента Hero
                if (other.gameObject.GetComponent<Hero>() != null)
                {
                    _action?.Invoke(other.gameObject);
                }
            }
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            // Аналогично для выхода из столкновения
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
