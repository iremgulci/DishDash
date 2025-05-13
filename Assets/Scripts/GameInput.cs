using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameInput : MonoBehaviour
{
public event EventHandler OnInteractAction; // Event handler for interaction
private PlayerInputActions playerInputActions;

private void Awake()
{
    playerInputActions = new PlayerInputActions (); // Create a new instance of PlayerInputActions
    playerInputActions.Player.Enable(); // Enable the Player input actions  
    playerInputActions.Player.Interact.performed += Interact_performed; // Subscribe to the Interact action performed event
}

private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
{
    // Handle the interaction logic here
    OnInteractAction?.Invoke(this, EventArgs.Empty); // Invoke the OnInteractAction event if not null
}
public Vector2 GetMovementVectorNormalized()
{
    Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>(); // Read the movement input from the Player input actions

    inputVector = inputVector.normalized; // Normalize the vector to ensure consistent movement speed
    
    return inputVector; // Return the movement vector
}
}
