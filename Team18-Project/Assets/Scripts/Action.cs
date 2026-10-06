using TMPro;
using UnityEngine;

public class Action : MonoBehaviour
{
    // GameManager
    GameManager gm;
    // Text
    TextMeshProUGUI buttonText;
    // Die
    Dielogic die;
    // Action Stuff
    public string attackName; // Name of Attack
    public int baseFailChance; // Base Chance to Fail
    public int failChance; // Chance to Fail
    public int power; // Amount of Damage/Healing it does
    public string failText; // Text when the player fails
    public string successText; // Text when the player succeeds


    void Start()
    {
        // Connect to Game Manager and Text of the Action Button
        gm = GameObject.FindGameObjectWithTag("Player").GetComponent<GameManager>();
        buttonText = GetComponentInChildren<TextMeshProUGUI>();
        // Reset Fail Chance
        failChance = baseFailChance;
    }
    void Update()
    {
        buttonText.text = $"{attackName}\nCyclops Eyes: {(failChance)}";
    }
    // Is used with a Button
    public void DoActionPlayer()
    {
        // If the player isn't ready, in between their action or the Ogre's Action
        if (gm.state!=GameManager.State.READY)
        {
            return;
        }
        // Game Manager get the action
        gm.CurrentAction = this;
        // Otherwise, do the action
        if (attackName=="Attack")
        {
            int result = die.RollDie();
        }
        Debug.Log("Did Action. YIPEE!!");
    }
}
