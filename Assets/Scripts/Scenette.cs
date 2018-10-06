using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scenette : MonoBehaviour {

    //Sequence and input section


    //Event section
    public delegate void voidFunc();
    public event voidFunc Sucess;
    public event voidFunc Fail;

    [SerializeField]
    float duration = 5;


    [Header("Input data and sequence part")]
    [SerializeField]
    public List<SequenceInput> sequencesToDo = new List<SequenceInput>();
    int currentSequenceIndex = 0;
    public List<Transform> positionForTouche = new List<Transform>();
    public bool finish = true;
    
    
    int _debug_indexPos = 0; // devra etre set a la main

    // Use this for initialization
    void Start () {
        SequenceInput currentSequence = new SequenceInput();
        for (int random = 0; random < Random.Range(3, 8); random++)
        {
            currentSequence.dI.Add(new DataInput((KeyCode)Random.Range(97, 122), positionForTouche[_debug_indexPos++].position));
            _debug_indexPos = (_debug_indexPos == (positionForTouche.Count - 1) ? 0 : _debug_indexPos);
        }
        sequencesToDo.Add(currentSequence);
        currentSequence = new SequenceInput();
        for (int random = 0; random < Random.Range(3, 8); random++)
        {
            currentSequence.dI.Add(new DataInput((KeyCode)Random.Range(97, 122), positionForTouche[_debug_indexPos++].position));
            _debug_indexPos = (_debug_indexPos == (positionForTouche.Count - 1) ? 0 : _debug_indexPos);
        }
        sequencesToDo.Add(currentSequence);
        currentSequence = new SequenceInput();
        for (int random = 0; random < Random.Range(3, 8); random++)
        {
            currentSequence.dI.Add(new DataInput((KeyCode)Random.Range(97, 122), positionForTouche[_debug_indexPos++].position));
            _debug_indexPos = (_debug_indexPos == (positionForTouche.Count - 1) ? 0 : _debug_indexPos);
        }
        sequencesToDo.Add(currentSequence);


        init();
    }

    public void init()
    {
        StartCoroutine(timerOfDuration(duration));
        currentSequenceIndex = -1;//car on fait un index++ au debut de next sequence
        nextSequence();
    }

    
    // Update is called once per frame
    void Update () {
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
                }
            }

        }
        else if( !finish )
        {
            nextSequence();
        }
        
        ////TOD DO DEBUG NOPE
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        /////End to do nope
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
