using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Components
{
    public class ChangeHealthComponent : MonoBehaviour
    {
        [SerializeField] private int _value;

        public void ApplyValue(GameObject target)
        {
            var healthComponent = target.GetComponent<HealthComponent>();
            if (healthComponent != null)
            {
                healthComponent.ApplyValue(_value);
            }
        }
    }

}
