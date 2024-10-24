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
        private int _dynamicSpeedMultipler = 50;
        private int _direction;

        private Rigidbody2D _rigidbody;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _direction = transform.localScale.x > 0 ? 1:-1;

            //rigidbody  = Dynamic
            var force = new Vector2(_direction * _speed * _dynamicSpeedMultipler, 0);
            _rigidbody.AddForce(force, ForceMode2D.Impulse);

        }

       
        //rigidbody = kinematic
        //private void FixedUpdate()
        //{
        //    var position = _rigidbody.position;
           
        //    position.x += _speed* _direction;
        //    _rigidbody.MovePosition(position);

        //}
    }

}
