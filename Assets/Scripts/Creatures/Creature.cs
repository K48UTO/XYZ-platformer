using UnityEngine;
using Scripts.Components.Particles;

namespace Scripts.Creatures
{
    public class Creature : MonoBehaviour
    {

        [SerializeField] private CheckCircleOverlaps _attackRange;
        [Header("Parameters")]
        [SerializeField] private bool _invertScale;
        [SerializeField] private float _speed;
        [SerializeField] protected float _jumpSpeed;
        [SerializeField] private float _damageJumpSpeed;


        [SerializeField] protected bool _allowDoubleJump;
        [SerializeField] protected bool _allowSuperJump = false;
        [SerializeField] protected float _SuperJumpBonus = 2;
        [SerializeField] float collisionForceThreshold = 5.0f; 

        [SerializeField] private LayerCheck _groundCheck;


        [SerializeField] protected LayerMask _interactionLayer;
        [SerializeField] protected float _groundCheckRadius;

        [SerializeField] protected SpawnListComponent _particles;

        [SerializeField] private bool _isFalling = false;
        [SerializeField] private bool _isTakingDamage = false; 
        [SerializeField] protected bool _isGrounded;
        [SerializeField] protected bool _isOnWall;

        protected Rigidbody2D _rigidbody;
        public Vector2 Direction;
        protected Animator _animator;

        private static readonly int IsGroundKey = Animator.StringToHash("is-ground");
        private static readonly int IsRunning = Animator.StringToHash("is-running");
        private static readonly int VerticalVelocity = Animator.StringToHash("vertical-velocity");
        private static readonly int Hit = Animator.StringToHash("hit");
        private static readonly int AttackKey = Animator.StringToHash("attack");

        protected virtual void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
        }

        public virtual void SetDirection(Vector2 directionVector)
        {
            Direction = directionVector;
        }

        protected virtual void FixedUpdate()
        {
            CalculateGrounded();
            var xVelocity = Direction.x * _speed;
            var yVelocity = CalculateYVelocity();
            _rigidbody.velocity = new Vector2(xVelocity, yVelocity);
            _animator.SetBool(IsGroundKey, _isGrounded);
            _animator.SetBool(IsRunning, Direction.x != 0);
            _animator.SetFloat(VerticalVelocity, _rigidbody.velocity.y);
            UpdateSpriteDirection(Direction);

            //ApplyGroundVelocity(); //Если нужно контролировать гравитацию героя через код
        }
        protected virtual float CalculateYVelocity()
        {
            var yVelocity = _rigidbody.velocity.y;
            var isJumpPressing = (Direction.y > 0) && (_isTakingDamage == false);


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
            else if (_allowDoubleJump && isFalling && !_isOnWall)
            {
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
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, 0);
        }
        public void UpdateSpriteDirection(Vector2 direction)
        {
            var multipler = _invertScale ? -1 : 1;
            if (direction.x > 0)
            {
                transform.localScale = new Vector3(multipler, 1, 1);
            }
            else if (direction.x < 0)
            {
                transform.localScale = new Vector3(-1 * multipler, 1, 1);

            }
        }
        public void ResetDamageFlag()
        {
            _isTakingDamage = false;
        }
        public virtual void Attack()
        {

            _animator.SetTrigger(AttackKey);

        }

        public virtual void OnDoAttack()
        {
            SpawnAttackDust();
            _attackRange.Check();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
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


        public void SpawnAttackDust()
        {
            _particles.Spawn("Attack");
        }



    }

}
