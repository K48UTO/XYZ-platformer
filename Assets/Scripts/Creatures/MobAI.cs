using System;
using System.Collections;
using System.Collections.Generic;
using Scripts.Components;
using UnityEngine;

namespace Scripts.Creatures
{
    public class MobAI : MonoBehaviour
    {
        [SerializeField] private LayerCheck _vision;
        [SerializeField] private LayerCheck _canAttack;
        [SerializeField] private LayerCheck _abyssCheck;



        [SerializeField] private float _alarmDelay = 0.5f;
        [SerializeField] private float _attackDelay = 1f;
        [SerializeField] private float _attackCoolDown = 1f;
        [SerializeField] private float _missHeroCooldown = 1f;




        private bool _isDead;
        private Patrol _patrol;

        private Coroutine _current;
        private GameObject _target;

        private static readonly int IsdeadKey = Animator.StringToHash("is-dead");

        private SpawnListComponent _particles;
        private Creature _creature;
        private Animator _animator;


        private void Awake()
        {
            _particles = GetComponent<SpawnListComponent>();
            _creature = GetComponent<Creature>();
            _animator = GetComponent<Animator>();
            _patrol = GetComponent<Patrol>();
            if (_patrol == null)
            {
                Debug.LogError("Patrol component is not attached to the GameObject.");
            }

        }
        private void Start()
        {
            StartState(_patrol.DoPatrol());
        }

        public void OnHeroVision(GameObject go)
        {
            if (_isDead) return;
            _target = go;
            SetDirectionToTarget();
            StartState(AgroToHero());
        }

        public void OnDie()
        {
            _isDead = true;
            _animator.SetBool(IsdeadKey, true);
            _creature.SetDirection(Vector2.zero);
            if (_current != null) StopCoroutine(_current);
        }

        public void ChangeBodySizeAfterDeath()
        {
            var ColliderBodySize = GetComponent<CapsuleCollider2D>();
            ColliderBodySize.size = new Vector2(0.6f, 0.6f);
        }



        private void StartState(IEnumerator coroutine)
        {
            _creature.SetDirection(Vector2.zero);
            if (_current != null)
            {
                StopCoroutine(_current);
            }
            _current = StartCoroutine(coroutine);


        }

        private IEnumerator AgroToHero()
        {
            _particles.Spawn("Exclamation");
            yield return new WaitForSeconds(_alarmDelay);

            StartState(GoToHero());
        }
        private IEnumerator GoToHero()
        {
            while (_vision.IsTouchingLayer)
            {
                if (_canAttack.IsTouchingLayer)
                {
                    StartState(Attack());
                }
                else
                {
                    if (IsAbyss())
                    {
                        _creature.SetDirection(Vector2.zero);
                    }
                    else
                    {
                        SetDirectionToTarget();
                    }
                }


                yield return null;
            }

            _particles.Spawn("MissHero");
            yield return new WaitForSeconds(_missHeroCooldown);

            
        }
        private void SetDirectionToTarget()
        {
            var direction = _target.transform.position - transform.position;
            direction.y = 0;
            _creature.SetDirection(direction.normalized);
        }
        private IEnumerator Attack()
        {
            while (_canAttack.IsTouchingLayer)
            {
                yield return new WaitForSeconds(_attackDelay);
                _creature.Attack();
                yield return new WaitForSeconds(_attackCoolDown);
            }
            StartState(GoToHero());
        }


        private bool IsAbyss()
        {
            return !_abyssCheck.IsTouchingLayer;
        }




    }

}
