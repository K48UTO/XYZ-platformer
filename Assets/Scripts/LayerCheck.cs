using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerCheck : MonoBehaviour
{
    public LayerMask Layer => _layer;

    [SerializeField] private LayerMask _layer;
    private Collider2D _colider;

    public bool IsTouchingLayer;

    private void Awake()
    {
        _colider = GetComponent<Collider2D>();  
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        IsTouchingLayer = _colider.IsTouchingLayers(_layer);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        IsTouchingLayer = _colider.IsTouchingLayers(_layer);

    }



}
