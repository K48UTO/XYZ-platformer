using UnityEngine;

namespace Assets.Scripts.Components
{
    public class SinusoidaProjectile : BaseProjectile
    {
        [SerializeField] private float _frequency = 1f;
        [SerializeField] private float _amplitude = 1f;
        private float _originalyY;
        private float _time;
        protected override void Start()
        {
            base.Start();
            _originalyY = Rigidbody.position.y;
        }
        private void FixedUpdate()
        {
            var position = Rigidbody.position;
            position.x += Direction * _speed;
            position.y = _originalyY + Mathf.Sin(_time * _frequency) * _amplitude;
            Rigidbody.MovePosition(position);
            _time += Time.fixedDeltaTime;
        }

    }
}