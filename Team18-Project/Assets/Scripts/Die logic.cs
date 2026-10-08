using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Dielogic : MonoBehaviour
{
    public Image dieFaceSprite;
    public Sprite[] dieFaces;
    public TextMeshProUGUI eyeChanceText;
    public AudioSource AudioSource;
    public Vector3 basePosition;
    public float shakeAmount = 20f;
    public Vector3 baseScale;
    public float scaleAmount = 0.10f;
    public float scaleDecrease = 0.01f;
    public float baseEyeChance = 0.10f; // Base Chance to roll the eye (percent)
    public float eyeChance; // Chance to roll the eye
    public bool rolling = false; //Is the roll anamation currently playing
    public float rollTime = 5f; //How long the die rolls for (secconds)
    float rollTimer = 0f; //amonut of time the die has been rolling
    int maxRollSpeed = 2; //fastest speed the die is alowed to roll at
    int rollSpeed; //speed that the die is rolling at (lower is faster)
    int rollIncrement = 4; //how quickly the die slows down
    int nextFace = 0; //counter for when to change the face
    public int dieFace = 6; //the current face of the die thats showing
    public int dieLanded; //the face it should land on when finished rolling
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        eyeChance = baseEyeChance;
        rollSpeed = maxRollSpeed;
        basePosition = transform.position;
        baseScale = transform.localScale;
        AudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //play the roll animation
        if (rolling)
        {
            //for a number of secconds
            if (rollTimer <= rollTime)
            {
                nextFace++;
                //only change the die face if a certain number of frames have passed
                if (nextFace >= rollSpeed)
                {
                    dieFace = Random.Range(2, 7); //do not show the eye

                    //SOUND EFFECT
                    if (!AudioSource.isPlaying)
                    {
                        AudioSource.Play();
                    }
                    rollSpeed += rollIncrement; //increase the number of frames between changes
                    nextFace = 0;
                    if (transform.position == basePosition)
                    {
                        transform.position += new Vector3(Random.Range(-shakeAmount, shakeAmount + 1), Random.Range(-shakeAmount, shakeAmount + 1), 0);
                    } else
                    {
                        transform.position = basePosition;
                    }
                }
                rollTimer += Time.deltaTime;

            } else //stop the animation and reset everything
            {
                rolling = false;
                rollSpeed = maxRollSpeed;
                nextFace = 0;
                rollTimer = 0f;
                dieFace = dieLanded;
                transform.position = basePosition;
                //start an animation of the die landing
                if (dieLanded == 1)
                {
                    transform.localScale += new Vector3(scaleAmount * 2, scaleAmount * 2, 0);
                } else
                {
                    transform.localScale += new Vector3(scaleAmount, scaleAmount, 0);
                }
            }
        }

        if (transform.localScale.x > baseScale.x)
        {
            if (dieLanded == 1)
            {
                transform.localScale -= new Vector3(scaleDecrease/5, scaleDecrease/5, 0);
            } else
            {
                transform.localScale -= new Vector3(scaleDecrease, scaleDecrease, 0);
            }
        }

        //draw the appropriate die sprite
        dieFaceSprite.sprite = dieFaces[dieFace - 1];
    }
    //returns the number the die rolls as an int; 1 is the eye
    public int RollDie()
    {
        //start the animation
        rolling = true;

        //eye chance overides anything
        if (Random.Range(0f, 1f) <= eyeChance)
        {
            dieLanded = 1;
            return dieLanded;
        } else //pick a normal die face at random
        {
            dieLanded = Random.Range(2, 7);
            return dieLanded;
        }
    }
}
