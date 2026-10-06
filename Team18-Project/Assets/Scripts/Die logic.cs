using UnityEngine;

public class Dielogic : MonoBehaviour
{
    public float baseEyeChance = 0.10f; // Base Chance to roll the eye (percent)
    public float eyeChance; // Chance to roll the eye
    public bool rolling = false; //Is the roll anamation currently playing
    public float rollTime = 3f; //How long the die rolls for (secconds)
    float rollTimer = 0f; //amonut of time the die has been rolling
    int maxRollSpeed = 1; //fastest speed the die is alowed to roll at
    int rollSpeed; //speed that the die is rolling at (lower is faster)
    int rollIncrement = 1; //how quickly the die slows down
    int nextFace = 0; //counter for when to change the face
    public int dieFace = 6; //the current face of the die thats showing
    public int dieLanded; //the face it should land on when finished rolling
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        eyeChance = baseEyeChance;
        rollSpeed = maxRollSpeed;
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
                    ////SOUND EFFECT
                    rollSpeed += rollIncrement; //increase the number of frames between changes
                    nextFace = 0;
                }
                rollTimer += Time.deltaTime;

            } else //stop the anumation and reset everything
            {
                rolling = false;
                rollSpeed = maxRollSpeed;
                nextFace = 0;
                rollTimer = 0f;
                dieFace = dieLanded;
                ////start an animation of the die landing
            }
        }

        //draw the appropriate die sprite
    }
    //returns the nomber the die rolls as an int; 1 is the eye
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
