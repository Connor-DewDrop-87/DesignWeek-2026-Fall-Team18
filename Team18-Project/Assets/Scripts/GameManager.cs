using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    // Block Reference
    public Action blockButton;
    // Player HP
    public float HPplayer = 10;
    public float maxHP;
    public GameObject HPFillPlayer;
    public GameObject HPFillPlayerDark;
    float playerPercentage = 1f;
    float playerPercentage2 = 1f;
    // Cyclop Stats
    float cyclopsPercentage = 1f;
    float cyclopsPercentage2 = 1f;
    public float HPCyclops = 50;
    public float maxHPCyclops;
    public GameObject HPFillCyclops;
    public GameObject HPFillCyclopsDark;
    public string cyclopsAttackType = "Normal";
    public float cyclopsAttackDammage = 0f;
    public float cyclopsNormalDammage = 3f;
    public float cyclopsNormalTH;
    public float cyclopsHeavyTH;
    public float cyclopsLaserTH;
    public float cyclopsPowerDammage = 7f;
    // Heart Sprites
    public Image HPHeartPlayer;
    public Image HPHeartCyclops;
    public Sprite[] heartSprites;
    public Sprite[] eyeSprites;
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
        HPFillPlayerDark = GameObject.Find("PlayerHPFill Dark");
        HPFillCyclopsDark = GameObject.Find("CyclopsHPFill Dark");
    }

    // Update is called once per frame
    void Update()
    {
        CheckHPUI();
        switch (state)
        {
            case GameManager.State.START: // Only Play at the Start
                messageDisplayer.text = startText + "\n(Click Here)";
                break;
            case GameManager.State.READY:
                CheckIfDeadOrWon();
                // Check Block Button for Die to get the Chance of Eye
                if (blockButton.die.eyeChance <= 0.1f) // Less than or equal to 10%
                {
                    messageDisplayer.text = "The cyclops guard is down, now's your chance to strike";
                }
                else if (blockButton.die.eyeChance <= 0.3f) // Less than or equal to 30%
                {
                    messageDisplayer.text = "You expect the cyclops to be growing impatient, don't become complacent";
                }
                else if (blockButton.die.eyeChance <= 0.5f) // Less than or equal to 50% 
                {
                    messageDisplayer.text = "You notice the cyclops becoming more erratic, don’t get distracted";
                }
                else // Greater than 50%
                {
                    messageDisplayer.text = "The cyclops is furious, keep your guard up";
                }
                // Go Back Here when the Player is able to Act
                // Different Flavour Text based on whatever attack the Cyclops will do Next
                if (cyclopsAttackType == "Normal")
                {
                    messageDisplayer.text += $"\nThe Cyclops is raising its Club";
                }
                else if (cyclopsAttackType == "Power")
                {
                    messageDisplayer.text += $"\nThe Cyclops is reaching for a Boulder";
                }
                else if (cyclopsAttackType == "Eye Ray")
                {
                    messageDisplayer.text += $"\nThe Cyclops is Preparing an Eye Ray";
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
                string sv = CheckShieldValue();
                messageDisplayer.text = $"{CurrentAction.failText[randomText]}\n{sv}\nCyclop's Turn (Click Here)";
                state = State.CYCLOPSTURN;
                break;
            case GameManager.State.CYCLOPSTEXT:
                
                // Different Flavour Texts based on what attack the cyclops did and whether or not it dealt damage
                if (cyclopsAttackDammage==0)
                {
                    messageDisplayer.text = $"You successfully block the Cyclop's Club";
                }
                else if (cyclopsAttackType == "Normal")
                {
                    
                    messageDisplayer.text = $"The Cyclops hits you with its club";
                }
                else if (cyclopsAttackType == "Power")
                {
                    messageDisplayer.text = $"The Cyclops batters you with a Boulder";
                }
                else if (cyclopsAttackType == "Eye Ray")
                {
                    messageDisplayer.text = $"The Cyclops shoots an Eye Ray at you";
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
                state = State.READY;
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
                float t = UnityEngine.Random.Range(0.000f, 1.000f);
                Debug.Log($"Rolled a :{t}");
                if (t < cyclopsNormalTH)
                {
                    cyclopsAttackType = "Normal";
                }
                else if (t < cyclopsHeavyTH)
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
            cyclopsAttackDammage = cyclopsNormalDammage * (1 - blockButton.currentBlockStrength);
        }
        else if (cyclopsAttackType == "Power")
        {
            Debug.Log("Power Attack");
            cyclopsAttackDammage = cyclopsPowerDammage * (1 - blockButton.currentBlockStrength);
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
        blockButton.currentBlockStrength = 0;

        state = State.CYCLOPSTEXT;
    }

    public string CheckShieldValue()
    {
        if(blockButton.currentBlockStrength >= 1)
        {
            return "In this time of peril your body knows what to do, your shield is masterfully placed to block most that is thrown your way";
        }
        if (blockButton.currentBlockStrength > 0.5 && blockButton.currentBlockStrength < 1)
        {
            return "Your shield is ready, you feel prepared, do not waver";
        }
        if (blockButton.currentBlockStrength >= 0.1 && blockButton.currentBlockStrength <= 0.5)
        {
            return "Your arm is heavy but still trying its best to keep your shield raised.";
        }
        return "You have neglected your shield and left it at your side, prepare for a world of hurt.";
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
        playerPercentage = HPplayer / maxHP;
        cyclopsPercentage = HPCyclops / maxHPCyclops;
        // Show UI
        HPFillPlayer.transform.localScale = new Vector3(playerPercentage, 1, 1);
        HPFillCyclops.transform.localScale = new Vector3(cyclopsPercentage, 1, 1);
        //Underbar
        if (playerPercentage2 > playerPercentage)
        {
            playerPercentage2 -= 0.10f * Time.deltaTime;
        }
        if (cyclopsPercentage2 > cyclopsPercentage)
        {
            cyclopsPercentage2 -= 0.05f * Time.deltaTime;
        }
        if (playerPercentage2 < 0)
        {
            playerPercentage = 0;
        }
        if (cyclopsPercentage2 < 0)
        {
            cyclopsPercentage2 = 0;
        }
        // Show UI
        HPFillPlayerDark.transform.localScale = new Vector3(playerPercentage2, 1, 1);
        HPFillCyclopsDark.transform.localScale = new Vector3(cyclopsPercentage2, 1, 1);
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
            HPHeartCyclops.sprite = eyeSprites[0];
        }
        else if (cyclopsPercentage>=0.25) // 25%-75% HP
        {
            HPHeartCyclops.sprite = eyeSprites[1];
        }
        else // Less than 25% HP
        {
            HPHeartCyclops.sprite = eyeSprites[2];
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
            if (cyclopsHPPercent<=0.5f)
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
            if (playerHPPercent >= 0.5f)
            {
                goodEnding = true;
            }
        }
    }
}
