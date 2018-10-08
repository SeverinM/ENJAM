using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scenette : MonoBehaviour {

    //Sequence and input section


    //Event section
    public delegate void voidFunc();
    public event voidFunc Sucess;
    public event voidFunc Fail;
    
    [Header("Data")]
    public bool startScene = false;
    public bool falseScene = false;
    public bool stickmou = false;
    //visual
    public Sprite backgroundSprite;
    public string textDefeat = "No !";
    public string textVictory = "Yes !";
    public int textSizeVictory = 10;
    public int textSizeDefeat = 10;


    public animHero animPourHero = animHero.attaqueAuDoigt;
    [SerializeField]
    float duration = 5;
    private Coroutine timerCoroutine;
    public bool finish = true;
    float timeRemain;
    int nb;

    public bool isWE = false;



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

    public void Awake()
    {
        if (falseScene)
            startScene = falseScene;
    }

    public void init()
    {
        nb = 0;
        Holder hold = Holder.instance;
        
        foreach(SequenceInput seq in sequencesToDo)
        {
            foreach(DataInput dI in seq.dI)
            {
                nb++;
            }
        }
        hold.setText(nb);

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
        if(!startScene)
            Holder.instance.setTime(duration, true);
        decor.sprite = backgroundSprite;
        if(!falseScene)
            hero.setAnimator(animPourHero);

        inputPressed = false;
        currentSequenceIndex = -1;//car on fait un index++ au debut de next sequence
    }

    public void startSequenceSequence()
    {
        if (!startScene)
            timerCoroutine = StartCoroutine(timerOfDuration(duration));
        if(!falseScene)
            nextSequence();

        //test holder part :
        Holder.instance.activate();
    }

    
    // Update is called once per frame
    void Update () {

        if (!finish && currentSequenceIndex != -1 && !falseScene)
        {
            InputHandling();
        }
            
        
        ////TOD DO DEBUG NOPE
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!falseScene)
                Application.Quit();
            else
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
                    dI.tFX.getFinish();
                    StartCoroutine(deleteThisOneFromCurrentSequence(dI));
                    forbiddenKeys.Add(dI.kc);
                    Holder.getInstance().Play(successClic);
                    Holder.instance.setText(--nb);

                    if (!inputPressed)
                    {
                        hero.startEnergitize();
                        inputPressed = true;
                    }
                }
            }

            foreach (char c in Input.inputString)
            {
                if (!forbiddenKeys.Contains((KeyCode)c) && (KeyCode)c != KeyCode.LeftShift && (KeyCode)c != KeyCode.RightShift)
                {
                    wrongChar++;
                }
            }
        }
        else
        {
            //print("nextSequence : "+ currentSequenceIndex + "for : " + this.gameObject.name);
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
            if (!isWE)
            {
                //Debug.Log((timeRemain / duration) * Holder.instance.honorMaxPerWin);
                Holder.instance.honor += (timeRemain / duration) * Holder.instance.honorMaxPerWin;
            }             
        }
        else
        {
            //Creation UI of next sequence
            foreach (DataInput dI in sequencesToDo[currentSequenceIndex].dI)
            {
                ToucheFX tf = Holder.instance.getKeyFx(dI.kc, dI.wPos).GetComponent<ToucheFX>();
                dI.tFX = tf;
                if (stickmou)
                {
                    dI.tFX.Stickmou();
                }
            }
        }
    }

    void NextScenette()
    {
        if (!startScene)
        {
            StopCoroutine(timerCoroutine);
            this.GetComponent<Animator>().SetTrigger("end");
        }
        else
            this.transform.position += Vector3.forward * 7;
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
        timeRemain = duration;
        Holder.instance.setTime(duration, true);
        while (timeRemain > 0)
        {
            timeRemain -= 0.1f + Time.deltaTime;
            if(wrongChar != 0)
            {
                timeRemain -= 0.4f * wrongChar;
                wrongChar = 0;
                Holder.instance.honor -= Holder.instance.honorLosePerTouch;
                Holder.instance.Play(failClic);

                //Ca fait bugger
                Holder.instance.shake += 1;
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
