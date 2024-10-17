using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementScript : MonoBehaviour
{
    //********** PRIVATE **********

    private Vector2 moveVector;
    private bool shouldMove;
    private bool shouldJump;
    private bool shouldDash;
    private bool shouldAttack;
    [SerializeField] private PlayerInput playerInput = null;

    //********** PUBLIC **********

    public Vector2 MoveVector { get { return moveVector; } }
    public bool ShouldMove { get { return shouldMove;} }
    public bool ShouldJump { get { return shouldJump;} }
    public bool ShouldDash { get { return shouldDash; } set { shouldDash = value; } }
    public bool ShouldAttack { get { return shouldAttack; } }
    public PlayerInput PlayerInput => playerInput;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        // Initializes variables
        moveVector = Vector2.zero;
        shouldMove = false;
        shouldJump = false;
        shouldDash = false;
    }

    // Called when the "GoLeft" action is triggered
    public void GoLeft(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            moveVector.x = -1;
            shouldMove = true;
        }
        else if (context.canceled && moveVector.x == -1)
        {
            moveVector.x = 0;
            shouldMove = false;
        }
    }

    // Called when the "GoRight" action is triggered
    public void GoRight(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            moveVector.x = 1;
            shouldMove = true;
        }
        else if (context.canceled && moveVector.x == 1)
        {
            moveVector.x = 0;
            shouldMove = false;
        }
    }

    // Called when the "Jump" action is triggered
    public void Jump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            // Set moveVector.y to 1 when the action is started (jumping up)
            moveVector.y = 1;
            shouldJump = true;
        }
        else if (context.canceled)
        {
            moveVector.y = 0;
            shouldJump = false;
        }
    }
    // Called when the "Dash" action is triggered
    public void Dash(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            shouldDash = true;
        }
    }
    // Called when the "Attack" action is triggered
    public void Attack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            shouldAttack = true;
        }
        if (context.canceled)
        {
            shouldAttack = false;
        }
    }
}
