using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private const string IS_WALKING = "IsWalking"; // Animator parameter name for walking state
    [SerializeField] private Player player; // Reference to the Player script
    private Animator animator; // Animator component
    
    private void Awake()
    {
        animator = GetComponent<Animator>(); // Get the Animator component attached to this GameObject
    }

    private void Update()
    {
        animator.SetBool(IS_WALKING, player.IsWalking()); // Update the walking state in the Animator based on the Player script
    }


}
