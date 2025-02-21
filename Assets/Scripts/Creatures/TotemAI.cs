using Scripts;
using Scripts.Components.Particles;
using UnityEditor.Animations;
using UnityEngine;

public class TotemAI : MonoBehaviour
{
    [SerializeField] private LayerCheck _vision;

    [Header("Parameters")]
    [SerializeField] private Cooldown _coolDown;
    [SerializeField] private SpawnComponent _rangeAttack;
    [SerializeField] private SpawnComponent _deadBodySpawner;


    public static readonly int Attack = Animator.StringToHash("attack");

    [SerializeField] private AnimatorController _mainTotem;
    [SerializeField] private AnimatorController _notMainTotem;
    [SerializeField] private GameObject _mainDeadTotem;
    [SerializeField] private GameObject _deadTotem;
    private Animator _animator;

    [SerializeField] private TotemAI _topTotem;
    [SerializeField] private TotemAI _bottomTotem;
    [SerializeField] private bool _isMain;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        SetAsMain();
    }
    private void Update()
    {
        if (_vision.IsTouchingLayer)
        {
            DetectPlayer();
        }
    }
    public void DetectPlayer()
    {
        if (_isMain && _coolDown.IsReady)
        {
            RangeAttack();
        }
        else
        {
            _topTotem?.DetectPlayer();
        }

    }
    public void RangeAttack()
    {
        _coolDown.Reset();
        if (_animator !=null)
        {
            _animator.SetTrigger(Attack); 
        }
    }
    public void OnRangeAttack()
    {
        _rangeAttack.Spawn();
        _bottomTotem?.RangeAttack();
    }
    private void SetAsMain()
    {
        _isMain = true;
        _animator.runtimeAnimatorController = _mainTotem;
        _deadBodySpawner.SetPrefabToSpawn(_mainDeadTotem);


    }
    private void UnsetAsMain()
    {

        _isMain = false;
        _animator.runtimeAnimatorController = _notMainTotem;
        _deadBodySpawner.SetPrefabToSpawn(_deadTotem);
    }
    public void TransferRole(GameObject totem)
    {
        _topTotem = totem.GetComponent<TotemAI>();
        _topTotem.SetBottomTotem(this.gameObject);
        UnsetAsMain(); 
    }
    private void SetTopTotem(GameObject other)
    {
        _topTotem = other.GetComponent<TotemAI>();
    }
    private void SetBottomTotem(GameObject other)
    {
        _bottomTotem = other.GetComponent<TotemAI>();
    }
}
