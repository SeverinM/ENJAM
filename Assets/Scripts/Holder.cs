using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Holder : MonoBehaviour {


    public float Vitesse = 0;
    public static Holder instance;

    private void Start()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public static Holder getInstance()
    {
        if (instance == null)
        {
            new GameObject().AddComponent<Holder>();
        }
        return instance;
    }
}
