using System.Collections;
using Scripts.Components.Health;
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
        [SerializeField] private CheckCircleOverlaps _interactionCheck;
        [SerializeField] private LayerCheck _wallCheck;


        [SerializeField] private AnimatorController _armed;
        [SerializeField] private AnimatorController _disarmed;

        public bool isButtonHeld;
        public float holdTime;
        public float throwInterval;
        private Coroutine _holdCoroutine;
        private Coroutine _throwingCoroutine;
        [SerializeField] private bool _isWallClose;        


        private bool isThrowing;



        [SerializeField] private Cooldown _throwCooldown;

        private static readonly int ThrowKey = Animator.StringToHash("throw");
        private static readonly int IsOnWall = Animator.StringToHash("is-on-wall");



        [Space]
        [Header("Particles")]
        //private Collider2D[] _interactionResult = new Collider2D[10];

        [SerializeField] ParticleSystem _hitParticles;

        [SerializeField] private Text CoinsTxt;



        private GameSession _session;

        public void OnHealthChanged(int currentHealth)
        {

            if (currentHealth != 0)
            {
                _session.Data.HP = currentHealth;
            }
        }


        private void Start()
        {
            _session = FindObjectOfType<GameSession>();
            var health = GetComponent<HealthComponent>();

            health.SetHealth(_session.Data.HP);
            UpdateHeroWeapon();

        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            _isWallClose = IsWallClose();
            var moveToSameDirection = Direction.x * transform.lossyScale.x > 0;
            if (_isWallClose && !_isGrounded && moveToSameDirection)
            {
                _isOnWall = true;
                _rigidbody.gravityScale = 0;

                _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, 0);  
                
                _animator.SetBool(IsOnWall, _isOnWall);
            }
            else
            {
                _isOnWall = false;
                _rigidbody.gravityScale = 1;
                _animator.SetBool(IsOnWall, _isOnWall);


            }

        }

        private bool IsWallClose()
        {
            return _wallCheck.IsTouchingLayer;
        }




        protected override float CalculateYVelocity()
        {
            if (_isGrounded) _allowDoubleJump = true;
            return base.CalculateYVelocity();
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
            _interactionCheck.Check();
        }

        public void SuperJumpPotion(int BonusValue)
        {
            _SuperJumpBonus = BonusValue;
            _allowSuperJump = true;
        }


        public override void Attack()
        {
            if (!_session.Data.IsArmed) return;
            base.Attack();
        }


        public void Throw(string name)
        {
            if (!_throwCooldown.IsReady) return;
            if (name == "started")
            {
                isButtonHeld = true;
                holdTime = 0f;
                _holdCoroutine = StartCoroutine(CountHoldTime());

            }
            else if (name == "canceled")
            {
                isButtonHeld = false;

                if (_holdCoroutine != null)
                {
                    StopCoroutine(_holdCoroutine);
                    if (holdTime >= 1f && _session.Data.Swords > 3)
                    {

                        _throwingCoroutine = StartCoroutine(ThrowSwordBurst());


                    }
                    else if (_session.Data.Swords > 1)
                    {

                        _animator.SetTrigger(ThrowKey);
                        _throwCooldown.Reset();

                    }


                }
            }




        }

        public void DoSingleThrow()
        {
            _particles.Spawn("Throw");
            RemoveSword(1);
        }


        internal void ArmHero()
        {

            if (_session.Data.Swords > 0)
            {
                _session.Data.IsArmed = true;
            }
            else
            {
                _session.Data.IsArmed = false;
            }
            UpdateHeroWeapon();
        }

        private void UpdateHeroWeapon()
        {


            _animator.runtimeAnimatorController = _session.Data.IsArmed ? _armed : _disarmed;

        }

        public void AddSword(int value)
        {
            _session.Data.Swords += value;
            ArmHero();
        }

        public void RemoveSword(int value)
        {
            _session.Data.Swords -= value;
            ArmHero();
        }

        private IEnumerator CountHoldTime()
        {
            while (isButtonHeld)
            {
                holdTime += Time.deltaTime;
                yield return null;
            }
        }

        private IEnumerator ThrowSwordBurst()
        {
            isThrowing = true;

            for (int i = 0; i < 3 && _session.Data.Swords > 1; i++)
            {
                _animator.SetTrigger(ThrowKey);
                yield return new WaitForSeconds(throwInterval);

            }

            isThrowing = false;
        }



    }
}