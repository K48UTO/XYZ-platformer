using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Scripts.Components
{
    public class InteractableComponent : MonoBehaviour
    {
        [SerializeField] private UnityEvent _action;

        public void Interact()
        {
            Debug.Log("Interact InteractableComponent called");
            _action?.Invoke();
        }

        [Serializable]
        public class EnterEvent : UnityEvent<GameObject>
        {

        }
    }

}