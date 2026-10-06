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
    public int rollsSinceEye = 0;


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
            if (!EyeLanded())
            {
                // Do Damage
                gm.HPCyclops -= die.dieLanded;
            }
        }
    }

    public bool EyeLanded()
    {
        if (die.dieLanded == 1)
        {
            gm.state = GameManager.State.FAILEDACTION;
            // Reset Eye Chance
            if (rollsSinceEye > 2)
            {
                die.eyeChance = die.baseEyeChance;
            }
            else if (rollsSinceEye == 2)
            {
                die.eyeChance = die.baseEyeChance * 0.75f;
            }
            else if (rollsSinceEye == 1)
            {
                die.eyeChance = die.baseEyeChance * 0.5f;
            }
            else
            {
                die.eyeChance = die.baseEyeChance * 0.1f;
            }
            rollsSinceEye = 0;
            return true;
        }
        else
        {
            gm.state = GameManager.State.SUCCESSFULACTION;
            // Increase Eye Chance
            die.eyeChance += die.baseEyeChance;
            rollsSinceEye++;
            return false;
        }
    }
}
