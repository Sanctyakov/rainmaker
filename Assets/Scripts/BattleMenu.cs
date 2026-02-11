using UnityEngine;
using UnityEngine.UI;

// Stores Battle Menu buttons and their functions. For example, Attack, Magic, Item, Run, etc. Each button will have a function that will be called when the button is pressed. The functions will be defined in the BattleManager script and will be called from this script when the buttons are pressed.

public class BattleMenu : MonoBehaviour
{
    public Button attackButton; // Button for the Attack action
    public Button magicButton; // Button for the Magic action
    public Button itemButton; // Button for the Item action
    public Button runButton; // Button for the Run action
    public Button defendButton; // Button for the Defend action
    public Button moveButton; // Button for the Move action

    public PlayerController playerController; // Reference to the PlayerController script

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
