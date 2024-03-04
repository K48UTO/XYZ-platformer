using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerCheck : MonoBehaviour
{
    public LayerMask GroundLayer => _groundLayer;

    [SerializeField] private LayerMask _groundLayer;
    private Collider2D _colider;

    public bool IsTouchingLayer;

    private void Awake()
    {
        _colider = GetComponent<Collider2D>();  
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        IsTouchingLayer = _colider.IsTouchingLayers(_groundLayer);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        IsTouchingLayer = _colider.IsTouchingLayers(_groundLayer);

    }



}
