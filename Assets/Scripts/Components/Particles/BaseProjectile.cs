using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Components
{
    public class BaseProjectile : MonoBehaviour
    {
        [SerializeField] protected float _speed;
        protected int _dynamicSpeedMultipler = 50;

        protected int Direction;
        protected Rigidbody2D Rigidbody;


        protected virtual void Start()
        {
            Rigidbody = GetComponent<Rigidbody2D>();
            Direction = transform.localScale.x > 0 ? 1 : -1;


        }

    }
}