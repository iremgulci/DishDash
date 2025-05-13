using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; } // Singleton instance of the Player class
    public event EventHandler <OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged; // Event handler for selected counter change
    public class OnSelectedCounterChangedEventArgs : EventArgs{
        public ClearCounter selectedCounter; // Selected counter
    }

    [SerializeField] private float moveSpeed = 7f; // Speed of the player
    [SerializeField] private GameInput gameInput; // Reference to the GameInput script
    [SerializeField] private LayerMask countersLayerMask; // Layer mask for counters
    
    private bool isWalking; 
    private Vector3 lastInteractDirection; // Last direction of interaction
    private ClearCounter selectedCounter; // Reference to the selected counter

    private void Awake()
    {
        if(Instance != null){
            Debug.LogError("There is more than one Player instance"); // Error if another instance exists
        }
        Instance = this; // Set the singleton instance to this object
    }

    private void Start()
    {
        gameInput.OnInteractAction += GameInput_OnInteractAction; // Listen to the interaction event
    }
    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {
        if(selectedCounter != null){
            selectedCounter.Interact(); // Call the Interact method of the selected counter
        }
    }
    // Update is called once per frame
    private void Update()
    {
        HandleMovement();
        HandleInteractions();

    }

    public bool IsWalking()
    {
        return isWalking; // Return the walking state of the player
    }

    private void HandleInteractions()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized(); // Get the normalized movement vector from GameInput
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        if(moveDir != Vector3.zero){
            lastInteractDirection = moveDir; // Update the last interaction direction
        }

        float interactDistance = 2f; // Distance for interaction
        if(Physics.Raycast(transform.position, lastInteractDirection, out RaycastHit raycastHit, interactDistance, countersLayerMask)){
            if(raycastHit.transform.TryGetComponent(out ClearCounter clearCounter)){
                //Has ClearCounter
                if(clearCounter != selectedCounter){
                    SetSelectedCounter(clearCounter); // Set the selected counter if it's different from the current one
                }
            } else {
                SetSelectedCounter(null); // No ClearCounter found
            }
        } else {
            SetSelectedCounter(null); // No ClearCounter found
        }
    }
    
    private void HandleMovement()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized(); // Get the normalized movement vector from GameInput
        Vector3 moveDirection = new Vector3(inputVector.x, 0f, inputVector.y);
        
        float moveDistance = moveSpeed * Time.deltaTime; // Calculate the distance to move based on speed and time 
        float playerRadius = 0.8f; 
        float playerHeight = 2f;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirection, moveDistance); // Check if the player can move in the desired direction using a capsule cast
        
        if(!canMove){
            //Cannot move towards moveDirrection

            //Attempt only to move on the X axis
            Vector3 moveDirectionX = new Vector3(moveDirection.x, 0, 0).normalized;
            canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirectionX, moveDistance);
            if(canMove){
                moveDirection = moveDirectionX; // Can move only on the X axis
            } else{
                //Cannot move only on the X axis
                //Attempt only to move on the Z axis
                Vector3 moveDirectionZ = new Vector3(0, 0, moveDirection.z);
                canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirectionZ, moveDistance);
                if(canMove){
                    moveDirection = moveDirectionZ; // Can move only on the Z axis
                }else{
                    //Cannot move in any direction
                }
            }
        }
        
        if(canMove){
            transform.position += moveDirection * moveDistance; // Move the player
        }
        
        isWalking = moveDirection != Vector3.zero; // Check if the player is walking
        
        float rotateSpeed = 10f; // Speed of rotation
        transform.forward = Vector3.Slerp(transform.forward, moveDirection, Time.deltaTime*rotateSpeed); // Rotate the player to face the direction of movement with smoothness
    }

    private void SetSelectedCounter(ClearCounter selectedCounter){
        this.selectedCounter = selectedCounter; // Set the selected counter
        
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs{
            selectedCounter = selectedCounter
        }); // Invoke the OnSelectedCounterChanged event with the new selected counter
    }
}
