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
    Color victory;

    [SerializeField]
    Color defeat;

	// Use this for initialization
	void Start ()
    {
        zDep = transform.position.z;
        transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height, zDep));

    }

    public void init(bool isWE , bool isDefeat, string txt, int sizeTxt = 15)
    {
        GetComponent<SpriteRenderer>().color = isDefeat ? defeat : victory;
        Debug.Log(transform.GetChild(0).name);
        transform.GetChild(1).GetChild(0).GetComponent<TextMesh>().text = txt;
        transform.GetChild(1).GetChild(0).GetComponent<TextMesh>().fontSize = sizeTxt;
        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = isWE ? WE : week;
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
            print("Here == zDep");
            transform.position -=  Vector3.forward * finalZ;
        }
	}

    private float zDep;
}
