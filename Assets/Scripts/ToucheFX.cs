using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToucheFX : MonoBehaviour {

    private Animator _anima;

    public void Start()
    {
        _anima = this.GetComponent<Animator>();
    }

    public void init(Vector2 worldPosition, string c )
    {
        GameObject child = transform.GetChild(0).gameObject;
        child.GetComponent<Text>().text = c;
        GetComponent<RectTransform>().anchorMin = worldPosition;
        GetComponent<RectTransform>().anchorMax = worldPosition;
        GetComponent<RectTransform>().position = new Vector3(100, 100, 0);
    }


    public void getFinish()
    {
        _anima.SetTrigger("success");
    }
   //getCall by the animation ! 
    public void endAnimation()
    {

    }
}
