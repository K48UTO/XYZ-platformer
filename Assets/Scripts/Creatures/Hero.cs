using Scripts.Components;
using Scripts.Model;
using Scripts.Utils;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Creatures
{
    public class Hero : Creature
    {

        [SerializeField] private float _interacionRadius;


        [SerializeField] private AnimatorController _armed;
        [SerializeField] private AnimatorController _disarmed;


        [Space]
        [Header("Particles")]
        [SerializeField] private SpawnComponent _footStepParticles;
        [SerializeField] private SpawnComponent _jumpParticles;
        [SerializeField] private SpawnComponent _fallParticles;
        [SerializeField] private SpawnComponent _AttackParticles1;

       
        private Collider2D[] _interactionResult = new Collider2D[10];

        [SerializeField] ParticleSystem _hitParticles;



        [SerializeField] private Text CoinsTxt;



        [SerializeField] private float _minFallSpeed = -10.0f;
        private float _maxFallSpeed = 0f;




      
        private bool _allowDoubleJump;
        private bool _allowSuperJump = false;
        private float _SuperJumpBonus = 2;

       

        private GameSession _session;


       protected override void Awake ()
        {
            base.Awake ();
        }
        public void OnHealthChanged(int currentHealth)
        {
            _session.Data.HP = currentHealth;
        }



        private void Start()
        {
            _session = FindObjectOfType<GameSession>();
            var health = GetComponent<HealthComponent>();
            health.SetHealth(_session.Data.HP);
            UpdateHeroWeapon();
        }

        //private readonly Collider2D[] _overlaps = new Collider2D[10];  //Если нужно контролировать гравитацию героя через код
        //private void ApplyGroundVelocity()
        //{
        //    int overlapsCount = _rigidbody.OverlapCollider(new ContactFilter2D { layerMask = _groundCheck.GroundLayer }, _overlaps);

        //    for (int i = 0; i < overlapsCount; i++)
        //    {
        //        if (_overlaps[i].TryGetComponent(out Rigidbody2D groundBody))
        //        {
        //            if (Physics.Raycast(_groundCheck.transform.position, Vector2.down, 5f, _groundCheck.GroundLayer.value))
        //            {
        //                return;
        //            }
        //        }  
        //    }
        //    _rigidbody.velocity += Physics2D.gravity * Time.fixedDeltaTime;
        //}
        protected override void FixedUpdate()
        {
            base.FixedUpdate();

        }

        public override void  SetDirection(Vector2 directionVector)
        {
            base.SetDirection(directionVector);
        }

        protected override float CalculateYVelocity()
        {
            if (_isGrounded) _allowDoubleJump = true;
            return base.CalculateYVelocity();
          
        }

        protected override float CalculateJumpVelocity(float yVelocity)
        {
            var isFalling = _rigidbody.velocity.y <= 0.001f;


            //if (!isFalling) return yVelocity;

            if (_isGrounded)
            {
                if (_allowSuperJump)
                {
                    yVelocity += _jumpSpeed * _SuperJumpBonus;
                    SpawnJumpDust();
                    _allowSuperJump = false;
                }
                else
                {
                    yVelocity += _jumpSpeed;
                    SpawnJumpDust();
                }
            }
            else if (_allowDoubleJump && isFalling)
            {
                Debug.Log("doubleJump");
                yVelocity = _jumpSpeed;
                SpawnJumpDust();
                _allowDoubleJump = false;
            }
            else
            {
                return yVelocity;
            }

            return yVelocity;
            

        }



        [SerializeField] float collisionForceThreshold = 5.0f; // Можно изменить на нужное значение

        private void OnCollisionEnter2D(Collision2D collision)
        {

            // Проверяем, достаточно ли сильное столкновение снизу (относительная скорость по Y)
            if (collision.relativeVelocity.y >= collisionForceThreshold)
            {
                SpawnFallDust();
            }
        }

      

        protected override void UpdateSpriteDirection()
        {
            base.UpdateSpriteDirection();
       
        }
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {


            Handles.color = IsGrounded() ? HandlesUtils.TransparentGreen : HandlesUtils.TransparentRed;
            Handles.DrawSolidDisc(transform.position, Vector3.forward, _groundCheckRadius);
        }
#endif


     

        public void AddCoin(int coin)
        {
            _session.Data.Coins += coin;
            CoinsTxt.text = _session.Data.Coins.ToString();

        }

        public override void TakeDamage()
        {
            base.TakeDamage();
            if (_session.Data.Coins > 0)
            {
                SpawnCoins();
            }

        }

        private void SpawnCoins()
        {
            var numCoinsToDispose = Mathf.Min(_session.Data.Coins, 5);
            _session.Data.Coins -= numCoinsToDispose;

            var burst = _hitParticles.emission.GetBurst(0);
            burst.count = numCoinsToDispose;
            _hitParticles.emission.SetBurst(0, burst);

            _hitParticles.gameObject.SetActive(true);
            _hitParticles.Play();
            CoinsTxt.text = _session.Data.Coins.ToString();


        }

   


        public void Interact()
        {
            var size = Physics2D.OverlapCircleNonAlloc(
                transform.position,
                _interacionRadius,
                _interactionResult,
                _interactionLayer);

            for (int i = 0; i < size; i++)
            {
                var interactable = _interactionResult[i].GetComponent<InteractableComponent>();
                if (interactable != null)
                {
                    Debug.Log("Interact method called from Hero");
                    interactable.Interact();
                }
            }
        }

        public void SuperJumpPotion(int BonusValue)
        {
            _SuperJumpBonus = BonusValue;
            _allowSuperJump = true;
        }
        public void SpawnFootDust()
        {
            _footStepParticles.Spawn();
        }
        public void SpawnJumpDust()
        {
            _jumpParticles.Spawn();
        }
        public void SpawnFallDust()
        {
            _fallParticles.Spawn();
        }
        public void SpawnAttackDust()
        {
            _AttackParticles1.Spawn();
        }

        public override void Attack()
        {
            if (!_session.Data.IsArmed) return;
            base.Attack();

        }

        public override void OnDoAttack()
        {
            SpawnAttackDust();
            base.OnDoAttack();
            
        }

        internal void ArmHero()
        {
            _session.Data.IsArmed = true;
            UpdateHeroWeapon();
        }

        private void UpdateHeroWeapon()
        {
            _animator.runtimeAnimatorController = _session.Data.IsArmed ? _armed : _disarmed;

        }
    }
}