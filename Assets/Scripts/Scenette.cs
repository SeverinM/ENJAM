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
    float time;
    

    [Header("Debug part")]
    public List<KeyCode> premiereSequence = new List<KeyCode>();
    public GameObject toucheFXPrefab;
    public List<Transform> positionForTouche = new List<Transform>();
    public List<ToucheFX> toucheFXoffCurrentSequence = new List<ToucheFX>();



    // Use this for initialization
    void Start () {
        
        premiereSequence.Add((KeyCode)Random.Range(97, 122));
        premiereSequence.Add((KeyCode)Random.Range(97, 122));
        premiereSequence.Add((KeyCode)Random.Range(97, 122));

        LaunchNextSequence();
    }
	
	// Update is called once per frame
	void Update () {


        int i = 0;
        foreach (KeyCode kc in premiereSequence)
        {
            if (Input.GetKeyDown(kc)) {

                toucheFXoffCurrentSequence[i].getFinish();
               //JAMAIS FAIRE CA !  premiereSequence.Remove(kc);
                //toucheFXoffCurrentSequence.Remove(toucheFXoffCurrentSequence[i]);
            }
            i++;
        }
		
	}

    void LaunchNextSequence()
    {
        int i = 0;
        foreach (KeyCode kc in premiereSequence)
        {
            ToucheFX tf = Instantiate(toucheFXPrefab).GetComponent<ToucheFX>();
            tf.init(positionForTouche[i++].position, kc.ToString());
            toucheFXoffCurrentSequence.Add(tf);
        }
    }

}
