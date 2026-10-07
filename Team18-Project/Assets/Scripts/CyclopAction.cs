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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("Player").GetComponent<GameManager>();
        sr = GetComponent<SpriteRenderer>();
        // Sprite 1 is the base Cyclops
        sr.sprite = cyclopsSprites[0];
    }

    // Update is called once per frame
    void Update()
    {
        if(gm.state == GameManager.State.CYCLOPSTEXT)
        {
            animationTime += Time.deltaTime;
            if (gm.cyclopsAttackType=="Eye Ray")
            {
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
            else
            {
                if (gm.cyclopsAttackType == "Normal")
                {
                    TimeBetweenSprites = 0.1f;
                }
                else
                {
                    TimeBetweenSprites = 0.05f;
                }   
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
            
        }
        else if (gm.state==GameManager.State.DEAD) // If the Player Died, Play a little Animation
        {
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
        else
        {
            animationTime = 0;
            currentSprite = 0;
        }
        Debug.Log($"Sprite: {currentSprite}");
        sr.sprite = cyclopsSprites[currentSprite];
    }
}
