using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scenette : MonoBehaviour {

    //Sequence and input section


    //Event section
    public delegate void voidFunc();
    public event voidFunc Sucess;
    public event voidFunc Fail;
    bool done = false;

    [SerializeField]
    float duration = 5;
    

    [Header("Debug part")]
    public List<DataInput> currentSequence = new List<DataInput>();
    public List<Transform> positionForTouche = new List<Transform>();
    public bool finish = true;

    float timeToEnter = 5;



    // Use this for initialization
    void Start () {

        currentSequence.Clear();
        currentSequence.Add( new DataInput((KeyCode)Random.Range(97, 122), positionForTouche[_debug_indexPos++].position) );
        currentSequence.Add( new DataInput((KeyCode)Random.Range(97, 122), positionForTouche[_debug_indexPos++].position) );
        currentSequence.Add( new DataInput((KeyCode)Random.Range(97, 122), positionForTouche[_debug_indexPos++].position) );
        init();
    }

    public void init()
    {
        StartCoroutine(timerOfDuration(duration));
        LaunchNextSequence();
    }

    int _debug_indexPos=0;

    int sizeCurrentSqc;
    // Update is called once per frame
    void Update () {
        sizeCurrentSqc = currentSequence.Count;
        if (currentSequence.Count != 0)
        {
            foreach (DataInput dI in currentSequence)
            {
                if (Input.GetKeyDown(dI.kc))
                {
                    dI.tFX.getFinish();
                    StartCoroutine(deleteThisOneFromCurrentSequence(dI));
                }
            }

        }
        else if( !finish )
        {
            for (int random = 0; random < Random.Range(3,5); random++)
            {
                currentSequence.Add(new DataInput((KeyCode)Random.Range(97, 122), positionForTouche[_debug_indexPos++].position));
                _debug_indexPos = (_debug_indexPos == 4 ? 0 : _debug_indexPos);
            }
            LaunchNextSequence();
        }

        if (currentSequence.Count == 0 && finish && !done)
        {
            if (!done)
            {
                done = true;
                Sucess();
            }
        }
    }
    int _debug_DBG = 0;
    void LaunchNextSequence()
    {
        //Creation UI of next sequence
        foreach (DataInput dI in currentSequence)
        {
            ToucheFX tf = Holder.instance.getKeyFx(dI.kc, dI.wPos).GetComponent<ToucheFX>();
            dI.tFX = tf;
        }
        _debug_DBG++;
    }

    void NextScenette()
    {
        finish = true;
    }


    IEnumerator deleteThisOneFromCurrentSequence(DataInput dI)
    {
        yield return new WaitForEndOfFrame();
        currentSequence.Remove(dI);
    }

    IEnumerator timerOfDuration(float duration)
    {
        float timeRemain = duration;
        while (timeRemain > 0 && !done)
        {
            Holder.instance.setTime(duration);
            timeRemain -= 0.1f + Time.deltaTime;
            Holder.instance.setTime(timeRemain, false);
            if (timeRemain <= 0)
            {
                FailTouche();
            }
            
            yield return new WaitForSeconds(0.1f);
        }
        Holder.instance.setTime(0);
    }

    void FailTouche()
    {
        Debug.Log("ji");
        //print("FAIIIIIIIIIIIL");
        foreach (DataInput dI in currentSequence)
        {
            dI.tFX.launchFail();
        }
        NextScenette();
        currentSequence.Clear();
        if (Fail != null)
        {
            done = true;
            Fail();
        }
    }
}
