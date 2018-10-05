using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToucheFX : MonoBehaviour {

    public Text charDisplay;

    private Animator _anima;

    public void Start()
    {
        _anima = this.GetComponent<Animator>();
    }

    public void init(Vector2 worldPosition, string c )
    {
        charDisplay.text = c;
        this.transform.position = worldPosition;
    }


    public void getFinish()
    {
        _anima.SetTrigger("success");
        Debug.Log("I do died.");
    }
   //getCall by the animation ! 
    public void endAnimation()
    {
        //destroy himself !!! In reality : touchePool deactivate him and keep him in stock for further use
    }
}
