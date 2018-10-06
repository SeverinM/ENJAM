using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bandeaux : MonoBehaviour {


    public AnimationCurve curve;
    public float maxTime;
    float time = 0;

	// Use this for initialization
	void Start () {
        transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height, 1));
	}
	
	// Update is called once per frame
	void Update () {
    
        if (time < maxTime)
        {
            time += Time.deltaTime;
            Vector3 vec = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, curve.Evaluate(time) * Screen.height, 1));
            transform.position = new Vector3(vec.x, vec.y, 1);
        }
	}
}
