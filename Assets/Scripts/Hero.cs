using System;
using System.Collections;
using System.Collections.Generic;
using Scripts;
using Scripts.Components;
using Scripts.Model;
using Scripts.Utils;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngineInternal;

public class Hero : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpSpeed;
    [SerializeField] private float _damageJumpSpeed;
    [SerializeField] private float _interacionRadius;
    [SerializeField] private float _groundCheckRadius;

    [SerializeField] private CheckCircleOverlaps _attackRange;

    [SerializeField] private int _damage;

    [SerializeField] private AnimatorController _armed;
    [SerializeField] private AnimatorController _disarmed;


    [Space]
    [Header("Particles")]
    [SerializeField] private SpawnComponent _footStepParticles;
    [SerializeField] private SpawnComponent _jumpParticles;
    [SerializeField] private SpawnComponent _fallParticles;
    [SerializeField] private SpawnComponent _AttackParticles1;

    [SerializeField] private LayerCheck _groundCheck;
    [SerializeField] private LayerMask _interactionLayer;
    private Collider2D[] _interactionResult = new Collider2D[10];

    [SerializeField] ParticleSystem _hitParticles;



    [SerializeField] private Text CoinsTxt;

    [SerializeField] private bool _isTakingDamage = false; // флаг состо€ни€ получени€ урона 

    [SerializeField] private bool _isFalling = false;

    [SerializeField] private float _minFallSpeed = -10.0f;
    private float _maxFallSpeed = 0f;




    private Rigidbody2D _rigidbody;
    private Vector2 _direction;
    private Animator _animator;
    [SerializeField] private bool _isGrounded;
    private bool _allowDoubleJump;
    private bool _allowSuperJump = false;
    private float _SuperJumpBonus = 2;

    private static readonly int IsGroundKey = Animator.StringToHash("is-ground");
    private static readonly int IsRunning = Animator.StringToHash("is-running");
    private static readonly int VerticalVelocity = Animator.StringToHash("vertical-velocity");
    private static readonly int Hit = Animator.StringToHash("hit");
    private static readonly int AttackKey = Animator.StringToHash("attack");

    private GameSession _session;


    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
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

    //private readonly Collider2D[] _overlaps = new Collider2D[10];  //≈сли нужно контролировать гравитацию геро€ через код
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
    private void FixedUpdate()
    {
        CalculateGrounded();
        var xVelocity = _direction.x * _speed;
        var yVelocity = CalculateYVelocity();
        _rigidbody.velocity = new Vector2(xVelocity, yVelocity);

        //ApplyGroundVelocity(); //≈сли нужно контролировать гравитацию геро€ через код

        _animator.SetBool(IsGroundKey, _isGrounded);
        _animator.SetBool(IsRunning, _direction.x != 0);
        _animator.SetFloat(VerticalVelocity, _rigidbody.velocity.y);


        UpdateSpriteDirection();

    }

    private void CalculateGrounded()
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

        private float CalculateYVelocity()
    {
        var yVelocity = _rigidbody.velocity.y;
        var isJumpPressing = (_direction.y > 0) && (_isTakingDamage == false);

        if (_isGrounded) _allowDoubleJump = true;
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

    private float CalculateJumpVelocity(float yVelocity)
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

    [SerializeField] float collisionForceThreshold = 5.0f; // ћожно изменить на нужное значение

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        // ѕровер€ем, достаточно ли сильное столкновение снизу (относительна€ скорость по Y)
        if (collision.relativeVelocity.y >= collisionForceThreshold)
        {
            SpawnFallDust();
        }
    }

    private bool IsGrounded()
    {
        return _groundCheck.IsTouchingLayer;
    }

    private void UpdateSpriteDirection()
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
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {


        Handles.color = IsGrounded() ? HandlesUtils.TransparentGreen : HandlesUtils.TransparentRed;
        Handles.DrawSolidDisc(transform.position, Vector3.forward, _groundCheckRadius);
    }
#endif


    public void SetDirection(Vector2 directionVector)
    {
        _direction = directionVector;
    }

    public void AddCoin(int coin)
    {
        _session.Data.Coins += coin;
        CoinsTxt.text = _session.Data.Coins.ToString();

    }

    public void TakeDamage()
    {
        _animator.SetTrigger(Hit);
        _isTakingDamage = true;
        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _damageJumpSpeed);

        Debug.Log("Taking damage called");
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

    public void ResetDamageFlag()
    {
        _isTakingDamage = false;
        Debug.Log("ResetDamageFlag called");

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

    public void Attack()
    {
        if (!_session.Data.IsArmed) return;
        _animator.SetTrigger(AttackKey);



    }

    public void OnDoAttack()
    {
        SpawnAttackDust();
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