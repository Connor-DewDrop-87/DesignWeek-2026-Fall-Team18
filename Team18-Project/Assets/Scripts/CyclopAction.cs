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
    public bool blackEyeShown = false;
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
            if (gm.cyclopsAttackType=="Normal")
            {
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
            if (gm.cyclopsAttackType=="Eye Ray")
            {
                if (blackEyeShown==true)
                {
                    TimeBetweenSprites = 0.2f;
                }
                else
                {
                    TimeBetweenSprites = 0.1f;
                }
                if (animationTime > TimeBetweenSprites && blackEyeShown==true)
                {
                    currentSprite=4;
                    animationTime = 0;
                    blackEyeShown = false;
                }
                if (animationTime > TimeBetweenSprites && blackEyeShown == false)
                {
                    currentSprite=1;
                    animationTime = 0;
                    blackEyeShown = true;
                }
            }
            
        }
        else
        {
            currentSprite = 0;
        }
        Debug.Log($"Sprite: {currentSprite}");
        sr.sprite = cyclopsSprites[currentSprite];
    }
}
