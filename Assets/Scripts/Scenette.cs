using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scenette : MonoBehaviour {

    //Sequence and input section


    //Event section
    public delegate void voidFunc();
    public event voidFunc Sucess;
    public event voidFunc Fail;
    
    [Header("Data")]
    public bool startScene = false;
    //visual
    [SerializeField]
    private Sprite backgroundSprite;
    [SerializeField]
    private animHero animPourHero = animHero.attaqueAuDoigt;
    [SerializeField]
    float duration = 5;
    private Coroutine timerCoroutine;
    public bool finish = true;



    [Header("Input data and sequence part")]
    [SerializeField]
    public List<SequenceInput> sequencesToDo = new List<SequenceInput>();
    private int currentSequenceIndex = 0;
    private int wrongChar = 0;
    public float timeBeforeNext = 0.5f;
    public float speedBandeauMultipler = 1;
    public float mutliplerSpeedEnter = 5;
    public bool inputPressed = false;



    [Header("Dont touch")]
    public Perso hero;
    public SpriteRenderer decor;

    [Header("Audio")]
    public AudioClip failClic;
    public AudioClip successClic;
    public AudioClip successScenette;
    
    
    public void init()
    {
        Holder hold = Holder.instance;

        if (failClic == null)
        {
            failClic = hold.failClic;
        }

        if (successClic == null)
        {
            successClic = hold.successClic;
        }

        if(successScenette == null)
        {
            successScenette = hold.soundsSuccess[Random.Range(0, hold.soundsSuccess.Count - 1)];
        }

        this.transform.position -= Vector3.forward;
        print("init by scenette, parfum pour bit");
        if (!startScene)
            timerCoroutine = StartCoroutine(timerOfDuration(duration));

        decor.sprite = backgroundSprite;
        hero.setAnimator(animPourHero);

        inputPressed = false;
        currentSequenceIndex = -1;//car on fait un index++ au debut de next sequence
        nextSequence();
    }

    
    // Update is called once per frame
    void Update () {

        if (!finish)
        {
            InputHandling();
        }
            
        
        ////TOD DO DEBUG NOPE
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        /////End to do nope
    }

    void InputHandling()
    {
        if (sequencesToDo[currentSequenceIndex].dI.Count != 0)
        {
            List<KeyCode> forbiddenKeys = new List<KeyCode>();
            foreach (DataInput dI in sequencesToDo[currentSequenceIndex].dI)
            {
                if (Input.GetKeyDown(dI.kc) && !forbiddenKeys.Contains(dI.kc))
                {
                    Debug.Log(dI.tFX);
                    dI.tFX.getFinish();
                    StartCoroutine(deleteThisOneFromCurrentSequence(dI));
                    forbiddenKeys.Add(dI.kc);
                    Holder.getInstance().Play(successClic);

                    if (!inputPressed)
                    {
                        hero.startEnergitize();
                        inputPressed = true;
                    }
                }
            }

            foreach (char c in Input.inputString)
            {
                if (!forbiddenKeys.Contains((KeyCode)c))
                {
                    wrongChar++;
                }
            }
        }
        else
        {
            nextSequence();
        }
    }


    void nextSequence()
    {
        currentSequenceIndex++;
        if(currentSequenceIndex == sequencesToDo.Count)
        {
            NextScenette();
            Sucess();
            Holder.instance.Play(successScenette);
        }
        else
        {
            //Creation UI of next sequence
            foreach (DataInput dI in sequencesToDo[currentSequenceIndex].dI)
            {
                ToucheFX tf = Holder.instance.getKeyFx(dI.kc, dI.wPos).GetComponent<ToucheFX>();
                dI.tFX = tf;
            }
        }
    }

    void NextScenette()
    {
        print("Yep");
        //degueulasse but will work
        foreach(Transform tf in GetComponentsInChildren<Transform>())
            tf.position += Vector3.forward;
        StopCoroutine(timerCoroutine);
        finish = true;
    }


    IEnumerator deleteThisOneFromCurrentSequence(DataInput dI)
    {
        yield return new WaitForEndOfFrame();
        sequencesToDo[currentSequenceIndex].dI.Remove(dI);
    }

    IEnumerator timerOfDuration(float duration)
    {
        yield return new WaitForEndOfFrame();
        float timeRemain = duration;
        Holder.instance.setTime(duration, true);
        while (timeRemain > 0)
        {
            timeRemain -= 0.1f + Time.deltaTime;
            if(wrongChar != 0)
            {
                timeRemain -= 0.4f * wrongChar;
                wrongChar = 0;
                Holder.instance.Play(failClic);
            }
            Holder.instance.setTime(timeRemain, false);
            yield return new WaitForSeconds(0.1f);
        }
        FailTouche();
        Holder.instance.setTime(0);
    }

    void FailTouche()
    {
        //Debug.Log("you faIIIIIIIIIIIIIIIIIIIIIIIILED !");
        foreach (DataInput dI in sequencesToDo[currentSequenceIndex].dI)
        {
            dI.tFX.launchFail();
        }
        NextScenette();
        sequencesToDo[currentSequenceIndex].dI.Clear();
        Fail();
    }
}
