using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{

    [SerializeField] private ClearCounter clearCounter; // Reference to the ClearCounter script
    [SerializeField] private GameObject visualGameObject; // Reference to the visual GameObject
    //Singleton instance pattern
    private void Start()
    {
        Player.Instance.OnSelectedCounterChanged += Player_OnSelectedCounterChanged; // Subscribe to the event
    }
    private void Player_OnSelectedCounterChanged(object sender, Player.OnSelectedCounterChangedEventArgs e)
    {
        if(e.selectedCounter == clearCounter){
            Show(); // Show the visual if the selected counter matches
        } else {
            Hide(); // Hide the visual if it doesn't match
        }
    }

    private void Show()
    {
        visualGameObject.SetActive(true); // Show the visual GameObject
    }   
    private void Hide()
    {
        visualGameObject.SetActive(false); // Hide the visual GameObject
    }


}
