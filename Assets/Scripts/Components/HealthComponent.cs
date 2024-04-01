using System;
using UnityEngine;
using UnityEngine.Events;

namespace Scripts.Components
{
    public class HealthComponent : MonoBehaviour
    {
        [SerializeField] private int _maxHealth;
        [SerializeField] private int _currentHealth;
        [SerializeField] private UnityEvent _onDamage;
        [SerializeField] private UnityEvent _onDie;
        [SerializeField] private HealthChangeEvent _onChange;


        [SerializeField] private bool _isInvincible = false;
        private bool _isDead = false; // ƒобавлено новое поле дл€ отслеживани€ состо€ни€ мертв/жив

      

        public void ApplyValue(int _value)
        {
            if (_isInvincible && _value < 0 || _isDead) return; // ƒобавлена проверка на состо€ние мертвости

            _currentHealth += _value;

            if (_value < 0)
            {
                _onDamage?.Invoke();
                Debug.Log("урона нанесено" + -_value);
            }

            if (_currentHealth <= 0 && !_isDead) // ƒобавлена проверка, чтобы событие смерти вызывалось только один раз
            {
                _onDie?.Invoke();
                _isDead = true; // ќбновл€ем состо€ние до мертвого
            }
            else if (_currentHealth > _maxHealth)
            {
                _currentHealth = _maxHealth;
            }
            _onChange.Invoke(_currentHealth);
        }

        public void MakeInvincible()
        {
            _isInvincible = true;
            Debug.Log("Make _isInvincibl" + _isInvincible);
        }

        public void RemoveInvincibility()
        {
            _isInvincible = false;
            Debug.Log("Remove _isInvincibl" + _isInvincible);
        }

#if UNITY_EDITOR
        [ContextMenu("UpdateHealth")]
        private void UpdateHealth ()
        {
            _onChange?.Invoke(_currentHealth);
        }
#endif

        internal void SetHealth(int health)
        {
            _currentHealth = health;
        }
    }
    [Serializable]
    public class HealthChangeEvent: UnityEvent<int> { }
}
