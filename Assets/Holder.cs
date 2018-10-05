using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Holder : MonoBehaviour {

    static Holder instance;
	
    public static Holder getInstance()
    {
        if (instance == null)
        {
            GameObject gob = new GameObject();
            instance = gob.AddComponent<Holder>();
        }
        return instance;
    }
}
