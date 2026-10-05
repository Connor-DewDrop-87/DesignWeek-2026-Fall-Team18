using TMPro;
using UnityEngine;

public class Action : MonoBehaviour
{
    // Player
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
    }
    void Update()
    {
        buttonText.text = name;
    }
    public void DoActionPlayer()
    {
        Debug.Log("Did Action. YIPEE!!");
    }
    public void DoActionCyclops()
    {

    }
}
