using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Player HP
    public int HPplayer = 10;
    public int maxHP;
    // Cyclop Stats
    public int HPCyclops = 50;
    public int maxHPCyclops;
    public enum State
    {
        START,
        READY,
        ROLLINGACTION,
        SUCCESSFULACTION,
        FAILEDACTION,
        CYCLOPSTURN,
        CYCLOPSTEXT,
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
        maxHP = HPplayer;
        maxHPCyclops = HPCyclops;
        state = State.START;
        // Get Message Displayer
        messageDisplayer = GameObject.Find("MainTextBox").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        // If the Player HP is below or at 0, kill the player
        if (HPplayer <= 0)
        {
            state = State.DEAD;
        }
        // If the Cyclops HP is below or at 0, the player wins
        if (HPCyclops<=0)
        {
            state = State.WON;
        }
        switch (state)
        {
            case GameManager.State.START: // Only Play at the Start
                messageDisplayer.text = startText + "\n(Click Here)";
                break;
            case GameManager.State.READY:
                // Go Back Here when the Player is able to Act
                messageDisplayer.text = "What do you do?\n(Click Actions Below)";
                break;
            case GameManager.State.ROLLINGACTION:
                messageDisplayer.text = $"Rolling...";
                // Dice Rolling Here
                break;
            case GameManager.State.SUCCESSFULACTION:
                messageDisplayer.text = $"{CurrentAction.successText}\n(Click Here)";
                break;
            case GameManager.State.FAILEDACTION:
                messageDisplayer.text = $"{CurrentAction.failText}\nCyclop's Turn (Click Here)";
                state = State.CYCLOPSTURN;
                break;
            case GameManager.State.CYCLOPSTEXT:
                messageDisplayer.text = $"The Cyclops Brings Its Club Down for 3 Damage\n(Click Here)";
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
            case GameManager.State.CYCLOPSTURN: // Deals damage on Click because, if it didn't
                                               // It would kill the player immediatly as it Update
                                               // Would Do it Every Frame
                HPplayer -= 3;
                state = State.CYCLOPSTEXT;
                break;
            case State.CYCLOPSTEXT:
                
                state = State.READY;
                break;
            case GameManager.State.WON:
                // Go to Win Screen (Do Later)
                break;
            case GameManager.State.DEAD:
                // Go to Death Screen (Do Later)
                break;
        }
    }
}
