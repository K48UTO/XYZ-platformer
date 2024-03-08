using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Scripts.Creatures;
using UnityEngine;
using UnityEngine.InputSystem;

public class HeroInputReader : MonoBehaviour
{
    [SerializeField] private Hero _hero;  

    public void OnMovement(InputAction.CallbackContext context)
    {
        Vector2 movementInput = context.ReadValue<Vector2>();
        _hero.SetDirection(movementInput);       
    }

    public void PressFire(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            Debug.Log(message: "something");
        }



    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            _hero.Interact();
        }
    }
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            _hero.Attack();
        }
    }

}
