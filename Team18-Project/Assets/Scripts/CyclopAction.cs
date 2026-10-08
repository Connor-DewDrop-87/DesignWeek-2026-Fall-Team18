using UnityEngine;

public class CyclopAction : MonoBehaviour
{
    GameManager gm;
    // Sprites
    SpriteRenderer sr;
    public Sprite[] cyclopsSprites;
    // Changing Sprites
    public float animationTime;
    public float TimeBetweenSprites = 0.2f;
    public int currentSprite;
    public bool swingingForward = true;
    // Audio Source
    AudioSource ass; // Audio Source Secretive
    public AudioClip[] clips; // Clips for Cyclops
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("Player").GetComponent<GameManager>();
        sr = GetComponent<SpriteRenderer>();
        // Sprite 1 is the base Cyclops
        sr.sprite = cyclopsSprites[0];
        ass = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(gm.state == GameManager.State.CYCLOPSTEXT)
        {
            int chosenActionSound = 0;
            
            animationTime += Time.deltaTime;
            if (gm.cyclopsAttackType=="Eye Ray")
            {
                chosenActionSound = 2;
                TimeBetweenSprites = 0.5f;
                if (animationTime <= TimeBetweenSprites/4)
                {
                    currentSprite = 1;
                }
                if (animationTime > TimeBetweenSprites/4 && animationTime <=  TimeBetweenSprites/2)
                {
                    currentSprite = 4;
                }
                if (animationTime > TimeBetweenSprites/2 && animationTime <=  TimeBetweenSprites*3/4)
                {
                    currentSprite = 5;
                }
                if (animationTime > TimeBetweenSprites*3/4 && animationTime <= TimeBetweenSprites)
                {
                    currentSprite = 6;
                }
                if (animationTime > TimeBetweenSprites && animationTime <=  TimeBetweenSprites*5/4)
                {
                    currentSprite = 5;
                }
                if (animationTime > TimeBetweenSprites * 5 / 4 && animationTime <= TimeBetweenSprites*3/2)
                {
                    currentSprite = 4;
                }
                if (animationTime > TimeBetweenSprites * 3 / 2 && animationTime <= TimeBetweenSprites*2)
                {
                    currentSprite = 1;
                }
                if (animationTime > TimeBetweenSprites*2)
                {
                    animationTime = 0;
                }
            }
            else if (gm.cyclopsAttackType == "Normal")
            {
                chosenActionSound = 3;
                TimeBetweenSprites = 0.1f; 
                if (animationTime > TimeBetweenSprites && swingingForward == true)
                {
                    currentSprite++;
                    animationTime = 0;
                    if (currentSprite >= 4)
                    {
                        currentSprite = 3;
                        swingingForward = false;
                    }
                }
                if (animationTime > TimeBetweenSprites && swingingForward == false)
                {
                    currentSprite--;
                    animationTime = 0;
                    if (currentSprite < 0)
                    {
                        currentSprite = 0;
                        swingingForward = true;
                    }
                }
            }
            else
            {
                chosenActionSound = 4;
                TimeBetweenSprites = 0.3f;
                if (animationTime <= TimeBetweenSprites / 4)
                {
                    currentSprite = 9;
                }
                if (animationTime > TimeBetweenSprites / 4 && animationTime <= TimeBetweenSprites / 2)
                {
                    currentSprite = 10;
                }
                if (animationTime > TimeBetweenSprites/ 2 && animationTime <= TimeBetweenSprites*3/4)
                {
                    currentSprite = 10;
                }
                if (animationTime > TimeBetweenSprites*3/ 4 && animationTime <= TimeBetweenSprites)
                {
                    currentSprite = 9;
                }
                if (animationTime > TimeBetweenSprites)
                {
                    animationTime = 0;
                }
            }
            if (gm.cyclopsAttackDammage==0)
            {
                chosenActionSound = 5;
            }
            if (ass.isPlaying == false)
            {
                ass.clip = clips[chosenActionSound];
                ass.Play();
            }
        }
        else if (gm.state==GameManager.State.DEAD) // If the Player Died, Play a little Animation
        {
            if (ass.isPlaying==false)
            {
                ass.clip = clips[0];
                ass.Play();
            }
            animationTime += Time.deltaTime;
            TimeBetweenSprites = 0.25f;
            if (animationTime > TimeBetweenSprites)
            {
                if (currentSprite==0)
                {
                    currentSprite = 7;
                }
                else
                {
                    currentSprite = 0;
                }
                animationTime = 0;
            }
        }
        else if (gm.state==GameManager.State.WON) // If the Player Won, show the Cyclops Dead
        {
            currentSprite = 8;
        }
        else if (gm.state==GameManager.State.SUCCESSFULACTION) // If the Player's action is successful, play a specific sound
        {
            if (ass.isPlaying == false)
            {
                if (gm.CurrentAction.actionName=="Attack")
                {
                    ass.clip = clips[1];
                    ass.Play();
                }

            }
            animationTime = 0;
            currentSprite = 0;
        }
        else if (gm.state==GameManager.State.FAILEDACTION) // If the Player's action is failed, play a specific sound
        {
            if (ass.isPlaying == false)
            {
                ass.clip = clips[0];
                ass.Play();
            }
            animationTime = 0;
            currentSprite = 0;
        }
        else
        {
            animationTime = 0;
            currentSprite = 0;
        }
        sr.sprite = cyclopsSprites[currentSprite];
    }
}
