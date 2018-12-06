using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bandeaux : MonoBehaviour {




    public AnimationCurve curve;
    public float maxTime;
    float time = 0;
    public float finalZ = 0.5f;

    [SerializeField]
    Sprite WE;

    [SerializeField]
    Sprite week;

    [SerializeField]
    Sprite bgVictory;

    [SerializeField]
    Sprite bgDefeat;

    [SerializeField]
    Color victory;

    [SerializeField]
    Color defeat;

    [SerializeField]
    SpriteRenderer toChange;

    [SerializeField]
    SpriteRenderer toChangeBG;

    [SerializeField]
    TextMesh transitionText;


    // Use this for initialization
    void Start ()
    {
        zDep = transform.position.z;
        transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height, zDep));

    }

    public void init(bool isWE , bool isDefeat, string txt, int sizeTxt = 15)
    {
        GetComponent<SpriteRenderer>().color = isDefeat ? defeat : victory;
        toChangeBG.sprite = isDefeat ? bgDefeat : bgVictory;
        txt = txt.Replace("\\j", "\n");
        transitionText.text = txt;
        transitionText.fontSize = sizeTxt;
        toChange.sprite = isWE ? WE : week;
    }
	
	// Update is called once per frame
	void Update () {
    
        if (time < maxTime)
        {
            time += Time.deltaTime * Holder.getInstance().getSpeed();
            Vector3 vec = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, curve.Evaluate(time) * Screen.height, zDep));
            transform.position = new Vector3(vec.x, vec.y, zDep);
        }
        else if(transform.position.z == zDep)
        {
            //print("Here == zDep");
            transform.position -=  Vector3.forward * finalZ;
        }
	}

    private float zDep;
}
