using System;
using UnityEngine;
using UnityEngine.Events;

namespace Scripts.Components.Health
{
    public class HealthComponent : MonoBehaviour
    {
        [SerializeField] private int _maxHealth;
        [SerializeField] private int _currentHealth;
        [SerializeField] private UnityEvent _onDamage;
        [SerializeField] private UnityEvent _onDie;
        [SerializeField] private HealthChangeEvent _onChange;


        [SerializeField] private bool _isInvincible = false;
        private bool _isDead = false; 

        public void ApplyValue(int _value)
        {
            if (_isInvincible && _value < 0 || _isDead) return; 

            _currentHealth += _value;

            if (_value < 0)
            {
                _onDamage?.Invoke();
            }

            if (_currentHealth <= 0 && !_isDead) 
            {
                _onDie?.Invoke();
                _isDead = true; 
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
        }

        public void RemoveInvincibility()
        {
            _isInvincible = false;
        }

#if UNITY_EDITOR
        [ContextMenu("UpdateHealth")]
        private void UpdateHealth ()
        {
            _onChange?.Invoke(_currentHealth);
        }
#endif

        public void SetHealth(int health)
        {
            _currentHealth = health;
        }
    }
    [Serializable]
    public class HealthChangeEvent: UnityEvent<int> 
    {
    }
}
