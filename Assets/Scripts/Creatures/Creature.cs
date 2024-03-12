using UnityEngine;
using Scripts.Components;

namespace Scripts.Creatures
{
    public class Creature : MonoBehaviour
    {

        [SerializeField] private CheckCircleOverlaps _attackRange;

        [SerializeField] private float _speed;
        [SerializeField] protected float _jumpSpeed;
        [SerializeField] private float _damageJumpSpeed;
        [SerializeField] private int _damage;

        [SerializeField] protected bool _allowDoubleJump;
        [SerializeField] protected bool _allowSuperJump = false;
        [SerializeField] protected float _SuperJumpBonus = 2;
        [SerializeField] float collisionForceThreshold = 5.0f; // ћожно изменить на нужное значение

        [SerializeField] private LayerCheck _groundCheck;
        [SerializeField] protected LayerMask _interactionLayer;
        [SerializeField] protected float _groundCheckRadius;

        [SerializeField] protected SpawnListComponent _particles;

        [SerializeField] private bool _isFalling = false;
        [SerializeField] private bool _isTakingDamage = false; // флаг состо€ни€ получени€ урона 
        [SerializeField] protected bool _isGrounded;

        protected Rigidbody2D _rigidbody;
        private Vector2 _direction;
        protected Animator _animator;

        private static readonly int IsGroundKey = Animator.StringToHash("is-ground");
        private static readonly int IsRunning = Animator.StringToHash("is-running");
        private static readonly int VerticalVelocity = Animator.StringToHash("vertical-velocity");
        private static readonly int Hit = Animator.StringToHash("hit");
        private static readonly int AttackKey = Animator.StringToHash("attack");

        protected virtual void Awake ()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
        }

        public virtual void SetDirection(Vector2 directionVector)
        {
            _direction = directionVector;
        }

        protected virtual void FixedUpdate()
        {
            CalculateGrounded();
            var xVelocity = _direction.x * _speed;
            var yVelocity = CalculateYVelocity();
            _rigidbody.velocity = new Vector2(xVelocity, yVelocity);
            _animator.SetBool(IsGroundKey, _isGrounded);
            _animator.SetBool(IsRunning, _direction.x != 0);
            _animator.SetFloat(VerticalVelocity, _rigidbody.velocity.y);
            UpdateSpriteDirection();

            //ApplyGroundVelocity(); //≈сли нужно контролировать гравитацию геро€ через код


        }
        protected virtual float CalculateYVelocity()
        {
            var yVelocity = _rigidbody.velocity.y;
            var isJumpPressing = (_direction.y > 0) && (_isTakingDamage == false);

            
            if (isJumpPressing)
            {

                yVelocity = CalculateJumpVelocity(yVelocity);


            }
            else if (_rigidbody.velocity.y > 0)
            {
                yVelocity *= 0.5f;

            }
            return yVelocity;
        }

        protected virtual float CalculateJumpVelocity(float yVelocity)
        {
            var isFalling = _rigidbody.velocity.y <= 0.001f;

            if (_isGrounded)
            {
                if (_allowSuperJump)
                {
                    yVelocity += _jumpSpeed * _SuperJumpBonus;

                    _allowSuperJump = false;
                }
                else
                {
                    yVelocity += _jumpSpeed;

                }
                _particles.Spawn("Jump");
            }
            else if (_allowDoubleJump && isFalling)
            {
                Debug.Log("doubleJump");
                yVelocity = _jumpSpeed;
                _particles.Spawn("Jump");
                _allowDoubleJump = false;
            }
            else
            {
                return yVelocity;
            }

            return yVelocity;

         

        }

        protected bool IsGrounded()
        {
            return _groundCheck.IsTouchingLayer;
        }
        protected virtual void CalculateGrounded()
        {
            _isGrounded = IsGrounded();

            if (!_isGrounded)
            {
                _isFalling = true;
            }
            else if (_isGrounded && _isFalling)
            {
                _isFalling = false;

            }

        }

        public virtual void TakeDamage()
        {
            _animator.SetTrigger(Hit);
            _isTakingDamage = true;
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _damageJumpSpeed);

            Debug.Log("Taking damage called");
          

        }
        protected virtual void UpdateSpriteDirection()
        {
            if (_direction.x > 0)
            {
                transform.localScale = Vector3.one;
            }
            else if (_direction.x < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);

            }
        }
        public void ResetDamageFlag()
        {
            _isTakingDamage = false;
            Debug.Log("ResetDamageFlag called");

        }
        public virtual void Attack()
        {
            
            _animator.SetTrigger(AttackKey);

        }

        public virtual void OnDoAttack()
        {
        
            var gos = _attackRange.GetObjectsInRange();
            foreach (var go in gos)
            {

                var hp = go.gameObject.GetComponent<HealthComponent>();
                if (hp != null && go.CompareTag("Barrel"))
                {

                    hp.ApplyValue(-_damage);

                }
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {

            // ѕровер€ем, достаточно ли сильное столкновение снизу (относительна€ скорость по Y)
            if (collision.relativeVelocity.y >= collisionForceThreshold)
            {
                SpawnFallDust();
            }
        }
        public void SpawnFallDust()
        {
            _particles.Spawn("Fall");
        }

        public void SpawnFootDust()
        {
            _particles.Spawn("Run");
        }






    }

}
