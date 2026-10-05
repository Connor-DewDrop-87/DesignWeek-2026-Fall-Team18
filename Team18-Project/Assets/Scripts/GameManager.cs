using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Player HP
    public int HP = 10;
    public int maxHP;
    // Cyclop Stats
    public int HPCyclops = 50;
    public int maxHPCyclops;
    public Action[] CyclopsActions;
    public enum State
    {
        START,
        READY,
        USINGACTION,
        SUCCESSFULACTION,
        FAILEDACTION,
        CYCLOPSTURN,
        CYCLOPSSUCCESSFUL,
        CYCLOPSFAILED,
        WON,
        DEAD
    }
    public State state;
    // Text
    TextMeshProUGUI messageDisplayer;
    public string startText; // Text when the player starts the game
    public string winText; // Text when the player defeats the Cyclops in the game
    public string deathText; // Text when the player dies in the game
    // For Recieving Actions from OptionManager
    public Action PreviousAction; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Set MaxHP to HP to prevent overhealing
        maxHP = HP;
        state = State.START;
        // Get Message Displayer
        messageDisplayer = GameObject.Find("MainTextBox").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (HP == 0)
        {
            state = State.DEAD;
        }
        switch (state)
        {
            case GameManager.State.START:
                messageDisplayer.text = startText;

                break;
            case GameManager.State.READY:
                // Nothing
                break;
            case GameManager.State.USINGACTION:
                messageDisplayer.text = $"You used: {PreviousAction.name}";
                break;
            case GameManager.State.SUCCESSFULACTION:
                messageDisplayer.text = $"{PreviousAction.successText}";
                state = State.CYCLOPSTURN;
                break;
            case GameManager.State.FAILEDACTION:
                messageDisplayer.text = $"{PreviousAction.failText}";
                state = State.CYCLOPSTURN;
                break;
            case GameManager.State.CYCLOPSTURN:

                break;
            case GameManager.State.CYCLOPSSUCCESSFUL:

                break;
            case GameManager.State.CYCLOPSFAILED:

                break;
            case GameManager.State.WON:
                messageDisplayer.text = winText;
                break;
            case GameManager.State.DEAD:
                messageDisplayer.text = deathText;
                break;
        }
    }
}
