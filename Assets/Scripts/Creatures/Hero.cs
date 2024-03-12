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
        private Collider2D[] _interactionResult = new Collider2D[10];

        [SerializeField] ParticleSystem _hitParticles;

        [SerializeField] private Text CoinsTxt;        

        private GameSession _session;
      
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

        //private readonly Collider2D[] _overlaps = new Collider2D[10];  //���� ����� �������������� ���������� ����� ����� ���
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
        public void SpawnAttackDust()
        {
            _particles.Spawn("Attack");
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