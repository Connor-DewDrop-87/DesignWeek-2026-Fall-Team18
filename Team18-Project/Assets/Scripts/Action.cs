using TMPro;
using UnityEngine;

public class Action : MonoBehaviour
{
    // GameManager
    GameManager gm;
    // Text
    TextMeshProUGUI buttonText;
    // Action Stuff
    public string name; // Name of Attack
    public int baseFailChance; // Base Chance to Fail
    public int failChance; // Chance to Fail
    public int power; // Amount of Damage/Healing it does
    public string failText; // Text when the player fails
    public string successText; // Text when the player succeeds


    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("Player").GetComponent<GameManager>();
        buttonText = GetComponentInChildren<TextMeshProUGUI>();
        baseFailChance = failChance;
    }
    void Update()
    {
        buttonText.text = name;
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
        if (name=="Attack")
        {
            
            // Roll up to 6 (Ints are exclusive, so 7 is 6)
            float dieRoll = Random.Range(1, 7);
            if (dieRoll > failChance) // If they succeed, increase the fail chance
            {
                failChance++;
                gm.state = GameManager.State.SUCCESSFULACTION;
            }
            else // If they fail, decrease the fail chance back to base
            {
                failChance = baseFailChance;
                gm.state = GameManager.State.FAILEDACTION;
            }
        }
        Debug.Log("Did Action. YIPEE!!");
    }
}
