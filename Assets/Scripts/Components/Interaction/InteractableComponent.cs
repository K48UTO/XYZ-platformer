using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Scripts.Components.Interaction
{
    public class InteractableComponent : MonoBehaviour
    {
        [SerializeField] private UnityEvent _action;

        public void Interact()
        {
            _action?.Invoke();
        }

        [Serializable]
        public class EnterEvent : UnityEvent<GameObject>
        {

        }
    }

}