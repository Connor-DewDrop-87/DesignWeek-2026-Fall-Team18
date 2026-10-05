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
    public Action CurrentAction;
    public float time = 0;
    public float maxAnimTime = 1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Set MaxHP to HP for both the player and Cyclops to prevent overhealing
        maxHP = HP;
        maxHPCyclops = HPCyclops;
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
        if (HPCyclops==0)
        {
            state = State.WON;
        }
        switch (state)
        {
            case GameManager.State.START:
                messageDisplayer.text = startText;
                break;
            case GameManager.State.READY:
                messageDisplayer.text = "What do you do?";
                break;
            case GameManager.State.USINGACTION:
                messageDisplayer.text = $"You used: {CurrentAction.name}";
                // Dice Rolling Here
                break;
            case GameManager.State.SUCCESSFULACTION:
                messageDisplayer.text = $"{CurrentAction.successText}";
                break;
            case GameManager.State.FAILEDACTION:
                messageDisplayer.text = $"{CurrentAction.failText}\nCyclop's Turn";
                state = State.CYCLOPSTURN;
                break;
            case GameManager.State.CYCLOPSTURN:
                // See ClickText
                break;
            case GameManager.State.WON:
                messageDisplayer.text = winText;
                break;
            case GameManager.State.DEAD:
                messageDisplayer.text = deathText;
                break;
        }
    }

    public void ClickText()
    {
        // When Clicked, change the state and, if its the Cyclops turn, do something
        switch (state)
        {
            case GameManager.State.START:
                state = State.READY;
                break;
            case GameManager.State.SUCCESSFULACTION:
                state = State.READY;
                break;
            case GameManager.State.FAILEDACTION:
                state = State.CYCLOPSTURN;
                break;
            case GameManager.State.CYCLOPSTURN:
                HP -= 3;
                state = State.READY;
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
