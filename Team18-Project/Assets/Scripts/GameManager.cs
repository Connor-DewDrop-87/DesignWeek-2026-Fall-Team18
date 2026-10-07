using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    Action act;
    // Player HP
    public float HPplayer = 10;
    public float maxHP;
    public GameObject HPFillPlayer;
    // Cyclop Stats
    public float HPCyclops = 50;
    public float maxHPCyclops;
    public GameObject HPFillCyclops;
    public string cyclopsAttackType = "Normal";
    public float cyclopsAttackDammage = 0f;
    public float cyclopsNormalDammage = 3f;
    public float cyclopsPowerDammage = 7f;
    // Heart Sprites
    public Image HPHeartPlayer;
    public Image HPHeartCyclops;
    public Sprite[] heartSprites;
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
    public string folkHeroEnding; // Text when the player defeats the Cyclops with greater than 15% HP
    public string fragileHeroEnding; // Text when the player defeats the Cyclops with less than 15% HP
    public string braveHeroEnding; // Text when the player is defeated by the Cyclops but the Cyclops is less than 15% HP
    public string arrogantHeroEnding; // Text when the player is defeated by the Cyclops but the Cyclops is more than 15% HP
    public bool goodEnding;
    // For Recieving Actions from OptionManager
    public Action CurrentAction;
    public float time = 0;
    public float maxAnimTime = 1;
    public int randomText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Set MaxHP to HP for both the player and Cyclops to prevent overhealing
        maxHP = HPplayer;
        maxHPCyclops = HPCyclops;
        state = State.START;
        // Get Message Displayer
        messageDisplayer = GameObject.Find("MainTextBox").GetComponent<TextMeshProUGUI>();
        // Connect to HPFills
        HPFillPlayer = GameObject.Find("PlayerHPFill");
        HPFillCyclops = GameObject.Find("CyclopsHPFill");
    }

    // Update is called once per frame
    void Update()
    {
        CheckHPUI();
        CheckIfDeadOrWon();
        switch (state)
        {
            case GameManager.State.START: // Only Play at the Start
                messageDisplayer.text = startText + "\n(Click Here)";
                break;
            case GameManager.State.READY:
                // Go Back Here when the Player is able to Act
                // Different Flavour Text based on whatever attack the Cyclops will do Next
                if (cyclopsAttackType == "Normal")
                {
                    messageDisplayer.text = $"The Cyclops is raising its Club";
                }
                else if (cyclopsAttackType == "Power")
                {
                    messageDisplayer.text = $"The Cyclops Winding-Up for a Powerful Strike";
                }
                else if (cyclopsAttackType == "Eye Ray")
                {
                    messageDisplayer.text = $"The Cyclops is Preparing an Eye Ray";
                }
                messageDisplayer.text += "\nWhat do you do?\n(Click Actions Below)";
                break;
            case GameManager.State.ROLLINGACTION:
                messageDisplayer.text = $"Rolling...";
                // Only do the action after the Dice has Stopped Rolling
                if (CurrentAction.die.rolling==false)
                {
                    CurrentAction.DoActionPlayer();
                }
                break;
            case GameManager.State.SUCCESSFULACTION:
                messageDisplayer.text = $"{CurrentAction.successText[randomText]}\n(Click Here)";
                break;
            case GameManager.State.FAILEDACTION:
                messageDisplayer.text = $"{CurrentAction.failText[randomText]}\nCyclop's Turn (Click Here)";
                state = State.CYCLOPSTURN;
                break;
            case GameManager.State.CYCLOPSTEXT:
                // Different Flavour Texts based on what attack the cyclops did and whether or not it dealt damage
                if (cyclopsAttackDammage==0)
                {
                    messageDisplayer.text = $"You successfully block the Cyclop's Attack";
                }
                else if (cyclopsAttackType == "Normal")
                {
                    messageDisplayer.text = $"The Cyclops hits you with its club for {cyclopsAttackDammage} Damage";
                }
                else if (cyclopsAttackType == "Power")
                {
                    messageDisplayer.text = $"The Cyclops batters you with its club {cyclopsAttackDammage} Damage";
                }
                else if (cyclopsAttackType == "Eye Ray")
                {
                    messageDisplayer.text = $"The Cyclops shoots an Eye Ray at you for {cyclopsAttackDammage} Damage";
                }
                messageDisplayer.text += $"\n(Click Here)";
                break;
            case GameManager.State.WON:
                if (goodEnding==true)
                {
                    messageDisplayer.text = "\t[Folk Hero Ending]\n"+folkHeroEnding + "\n(Click Here)";
                }
                else
                {
                    messageDisplayer.text = "\t[Brittle Hero Ending]\n" + fragileHeroEnding + "\n(Click Here)";
                }

                break;
            case GameManager.State.DEAD:
                if (goodEnding == true)
                {
                    messageDisplayer.text = "\t[Brave Hero Ending]\n" + braveHeroEnding + "\n(Click Here)";
                }
                else
                {
                    messageDisplayer.text = "\t[Arrogant Hero Ending]\n" + arrogantHeroEnding + "\n(Click Here)";
                }
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
                if (CurrentAction.actionName=="Block")
                {
                    PrepareCyclopsTurn();
                    
                }
                else
                {
                    state = State.READY;
                }   
                break;
            case GameManager.State.FAILEDACTION:
                state = State.CYCLOPSTURN;
                break;
            case GameManager.State.CYCLOPSTURN: // Deals damage on Click because, if it didn't
                                                // It would kill the player immediatly as it Update
                                                // Would Do it Every Frame
                PrepareCyclopsTurn();
                break;
            case State.CYCLOPSTEXT:
                //Determine the what the next attack will be
                //Done here so that the player can have warning and so the first attack is always normal
                float t = Random.Range(0f, 1f);
                if (t < 0.5f)
                {
                    cyclopsAttackType = "Normal";
                }
                else if (t < 0.75f)
                {
                    cyclopsAttackType = "Power";
                }
                else
                {
                    cyclopsAttackType = "Eye Ray";
                }
                state = State.READY;
                break;
            case GameManager.State.WON:
                SceneManager.LoadScene("TitleScreen");
                break;
            case GameManager.State.DEAD:
                SceneManager.LoadScene("TitleScreen");
                break;
        }
    }
    // Cyclops Turn is put here so that it can be referenced for Blocking
    public void PrepareCyclopsTurn()
    {
        if (cyclopsAttackType == "Normal")
        {
            Debug.Log("Normal Attack");
            cyclopsAttackDammage = cyclopsNormalDammage * (1 - CurrentAction.currentBlockStrength);
        }
        else if (cyclopsAttackType == "Power")
        {
            Debug.Log("Power Attack");
            cyclopsAttackDammage = cyclopsPowerDammage * (1 - CurrentAction.currentBlockStrength);
        }
        else if (cyclopsAttackType == "Eye Ray")
        {
            Debug.Log("Laser");
            cyclopsAttackDammage = cyclopsNormalDammage;
        }
        // If the damage is below 0, reset to 0 
        if (cyclopsAttackDammage < 0)
        {
            cyclopsAttackDammage = 0;
        }
        //do the dammage and reset the block
        HPplayer -= cyclopsAttackDammage;
        CurrentAction.currentBlockStrength = 0;

        state = State.CYCLOPSTEXT;
    }

    public void CheckHPUI()
    {
        // Check for Over MaxHP and Under 0 HP
        if (HPplayer<0)
        {
            HPplayer = 0;
        }
        if (HPplayer>maxHP)
        {
            HPplayer = maxHP;
        }
        if (HPCyclops<0)
        {
            HPCyclops = 0;
        }
        if (HPCyclops>maxHPCyclops)
        {
            HPCyclops = maxHPCyclops;
        }
        // Get the HP Percentage for Scaling UI
        float playerPercentage = HPplayer / maxHP;
        float cyclopsPercentage = HPCyclops / maxHPCyclops;
        // Show UI
        HPFillPlayer.transform.localScale = new Vector3(playerPercentage, 1, 1);
        HPFillCyclops.transform.localScale = new Vector3(cyclopsPercentage, 1, 1);
        // Check if Sprite Change for Heart is Needed
        // Player
        if (playerPercentage>=0.75) // 75% or more HP
        {
            HPHeartPlayer.sprite = heartSprites[0];
        }
        else if (playerPercentage>=0.25) // 25%-75% HP
        {
            HPHeartPlayer.sprite = heartSprites[1];
        }
        else // Less than 25% HP
        {
            HPHeartPlayer.sprite = heartSprites[2];
        }
        // Cyclops
        if (cyclopsPercentage>=0.75) // 75% or more HP
        {
            HPHeartCyclops.sprite = heartSprites[0];
        }
        else if (cyclopsPercentage>=0.25) // 25%-75% HP
        {
            HPHeartCyclops.sprite = heartSprites[1];
        }
        else // Less than 25% HP
        {
            HPHeartCyclops.sprite = heartSprites[2];
        }
    }
    public void CheckIfDeadOrWon()
    {
        // If the Player HP is below or at 0, kill the player
        if (HPplayer <= 0)
        {
            state = State.DEAD;
            float cyclopsHPPercent = HPCyclops / maxHPCyclops;
            Debug.Log($"Cyclops: {cyclopsHPPercent}");
            if (cyclopsHPPercent<=0.25f)
            {
                goodEnding = true;
            }
        }
        // If the Cyclops HP is below or at 0, the player wins
        if (HPCyclops <= 0)
        {
            state = State.WON;
            float playerHPPercent = HPplayer / maxHP;
            Debug.Log($"Player: {playerHPPercent}");
            if (playerHPPercent >= 0.25f)
            {
                goodEnding = true;
            }
        }
    }
}
