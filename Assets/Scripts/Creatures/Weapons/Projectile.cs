using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using Unity.VisualScripting;
using UnityEngine;

namespace Scripts.Creatures.Weapons
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private GameObject _hero;
        private int _direction;

        private Rigidbody2D _rigidbody;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _direction = transform.localScale.x > 0 ? 1:-1;
        }

        private void FixedUpdate()
        {
            var position = _rigidbody.position;
           
            position.x += _speed* _direction;
            _rigidbody.MovePosition(position);

        }
    }

}
