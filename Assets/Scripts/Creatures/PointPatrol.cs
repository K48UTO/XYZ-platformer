using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Creatures
{
    public class PointPatrol : Patrol
    {

        [SerializeField] private Transform[] _points;
        [SerializeField] private LayerCheck _abyssCheck;
        [SerializeField] private float _treshold = 1f;

        private Creature _creature;
        private int _destinationPointIndex;

        private void Awake()
        {
            _creature = GetComponent<Creature>();

        }

        public override IEnumerator DoPatrol()
        {
            while (enabled)
            {
                if (isOnPoint() || IsAbyss())
                {
                    _destinationPointIndex = (int)Mathf.Repeat(_destinationPointIndex + 1, _points.Length);
                }
                var direction = _points[_destinationPointIndex].position - transform.position;
                direction.y = 0;
                _creature.SetDirection(direction.normalized);
                yield return null;
            }
        }
       
        private bool isOnPoint ()
        {
            return (_points[_destinationPointIndex].position - transform.position).magnitude < _treshold;
        }

        private bool IsAbyss()
        {
            return !_abyssCheck.IsTouchingLayer;
        }

    }

}
