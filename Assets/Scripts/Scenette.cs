﻿using System.Collections;
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
    public bool inputPressed = false;



    [Header("Dont touch")]
    public Perso hero;
    public SpriteRenderer decor;

    int _debug_indexPos = 0; // devra etre set a la main

    public float mutliplerSpeedEnter = 5;
    
    
    public void init()
    {
        Holder.getInstance().setSpeed(speedBandeauMultipler);
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
                    dI.tFX.getFinish();
                    StartCoroutine(deleteThisOneFromCurrentSequence(dI));
                    forbiddenKeys.Add(dI.kc);

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
        float timeRemain = duration;
        Holder.instance.setTime(duration, true);
        while (timeRemain > 0)
        {
            timeRemain -= 0.1f + Time.deltaTime;
            if(wrongChar != 0)
            {
                timeRemain -= 0.4f * wrongChar;
                wrongChar = 0;
                Holder.instance.timerHurt();
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
