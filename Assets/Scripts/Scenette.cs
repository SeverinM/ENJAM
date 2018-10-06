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
    

    [Header("Debug part")]
    public List<DataInput> premiereSequence = new List<DataInput>();
    public List<Transform> positionForTouche = new List<Transform>();



    // Use this for initialization
    void Start () {
        
        premiereSequence.Add( new DataInput((KeyCode)Random.Range(97, 122), positionForTouche[0].position) );
        premiereSequence.Add( new DataInput((KeyCode)Random.Range(97, 122), positionForTouche[1].position) );
        premiereSequence.Add( new DataInput((KeyCode)Random.Range(97, 122), positionForTouche[2].position) );

        init();
    }

    public void init()
    {
        StartCoroutine(timerOfDuration(duration));
        LaunchNextSequence();
    }



    // Update is called once per frame
    void Update () {


        int i = 0;
        foreach (DataInput dI in premiereSequence)
        {
            if (Input.GetKeyDown(dI.kc)) {

                dI.tFX.getFinish();
                
            }
            i++;
        }
		
	}

    void LaunchNextSequence()
    {


        //Creation UI of next sequence
        foreach (DataInput dI in premiereSequence)
        {
            ToucheFX tf = Holder.instance.getKeyFx(dI.kc, dI.wPos).GetComponent<ToucheFX>();
            dI.tFX = tf;
        }
    }


    IEnumerator timerOfDuration(float duration)
    {
        float timeRemain = duration;
        while(duration > 0)
        {
            timeRemain -= 0.1f + Time.deltaTime;
            Holder.instance.setTime(timeRemain/5);
            yield return new WaitForSeconds(0.1f);
        }
        Holder.instance.setTime(0);

    }


}
