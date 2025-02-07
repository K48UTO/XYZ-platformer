using Scripts;
using Scripts.Components.Particles;
using UnityEngine;

public class ShootingTrapAI : MonoBehaviour
{
    [SerializeField] private LayerCheck _vision;

    [Header("melee")]
    [SerializeField] private Cooldown _meleeCooldown;
    [SerializeField] private CheckCircleOverlaps _meleeAttack;
    [SerializeField] private LayerCheck _meleeCanAttack;

    [Header("range")]
    [SerializeField] private Cooldown _rangeCooldown;
    [SerializeField] private SpawnComponent _rangeAttack;

    public static readonly int Melee = Animator.StringToHash("melee");
    public static readonly int Range = Animator.StringToHash("range");

    private Animator _animator;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (_vision.IsTouchingLayer)
        {
            if (_meleeCanAttack.IsTouchingLayer)
            {
                if (_meleeCooldown.IsReady)
                {
                    MeleeAttack();
                }
                    return;
            }

            if (_rangeCooldown.IsReady)
            {
                RangeAttack();
            }
        }
    }



    private void RangeAttack()
    {
        _rangeCooldown.Reset();
        _animator.SetTrigger(Range);
    }

    private void MeleeAttack()
    {
        _meleeCooldown.Reset();
        _animator.SetTrigger(Melee);
    }

    public void OnMelleAttack()
    {
        _meleeAttack.Check();
    }

    public void OnRangeAttack()
    {
        _rangeAttack.Spawn();
    }
}
