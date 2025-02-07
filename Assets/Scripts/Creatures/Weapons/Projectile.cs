using Assets.Scripts.Components;
using UnityEngine;

namespace Scripts.Creatures.Weapons
{
    public class Projectile : BaseProjectile
    {
        protected override void Start()
        {
            base.Start();
            //rigidbody  = Dynamic
            var force = new Vector2(Direction * _speed * _dynamicSpeedMultipler, 0);
            Rigidbody.AddForce(force, ForceMode2D.Impulse);

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
