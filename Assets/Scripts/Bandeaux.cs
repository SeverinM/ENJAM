using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bandeaux : MonoBehaviour {


    public AnimationCurve curve;
    public float maxTime;
    float time = 0;
    public float finalZ = 0.5f;

	// Use this for initialization
	void Start ()
    {
        zDep = transform.position.z;
        transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height, zDep));

    }
	
	// Update is called once per frame
	void Update () {
    
        if (time < maxTime)
        {
            time += Time.deltaTime * Holder.getInstance().getSpeed();
            Vector3 vec = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, curve.Evaluate(time) * Screen.height, zDep));
            transform.position = new Vector3(vec.x, vec.y, 1);
        }
        else if(transform.position.z == zDep)
        {
            transform.position -=  Vector3.forward * finalZ;
        }
	}

    private float zDep;
}
