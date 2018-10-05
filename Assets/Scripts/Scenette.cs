using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scenette : MonoBehaviour {

    public delegate void voidFunc();
    public event voidFunc Sucess;
    public event voidFunc Fail;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
