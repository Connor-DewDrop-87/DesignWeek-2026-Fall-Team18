using TMPro;
using UnityEngine;

public class Action : MonoBehaviour
{
    // GameManager
    GameManager gm;
    // Text
    TextMeshProUGUI buttonText;
    // Die
    public Dielogic die;
    // Action Stuff
    public string attackName; // Name of Attack
    public int power; // Amount of Damage/Healing it does
    public string failText; // Text when the player fails
    public string successText; // Text when the player succeeds


    void Start()
    {
        // Connect to Game Manager and Text of the Action Button
        gm = GameObject.FindGameObjectWithTag("Player").GetComponent<GameManager>();
        buttonText = GetComponentInChildren<TextMeshProUGUI>();
        die = GetComponent<Dielogic>();
    }
    void Update()
    {
        buttonText.text = $"{attackName}\nEye Chance: {die.eyeChance*100}%";
    }
    // Is used with a Button
    public void SelectActionPlayer()
    {
        // If the player isn't ready, in between their action or the Ogre's Action
        if (gm.state!=GameManager.State.READY)
        {
            return;
        }
        // Game Manager get the action
        gm.CurrentAction = this;
        int result = die.RollDie();
        gm.state = GameManager.State.ROLLINGACTION;
        Debug.Log("Did Action. YIPEE!!");
    }

    public void DoActionPlayer()
    {
        if (attackName=="Attack")
        {
            if (die.dieLanded == 1)
            {
                gm.state = GameManager.State.FAILEDACTION;
                // Reset Eye Chance
                die.eyeChance = die.baseEyeChance;
            }
            else
            {
                gm.state = GameManager.State.SUCCESSFULACTION;
                // Increase Eye Chance
                die.eyeChance += die.baseEyeChance;
                // Do Damage
                gm.HPCyclops -= die.dieLanded;
            }
        }
    }
}
