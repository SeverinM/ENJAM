using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToucheFX : MonoBehaviour {

    [SerializeField]
    private Text textChar;

    private Animator _anima;
    private RectTransform _rectTransform;

    public void Awake()
    {
        _anima = this.GetComponent<Animator>();
        _rectTransform = this.GetComponent<RectTransform>();
    }

    public void init(Vector2 worldPosition, string c )
    {
        GameObject child = transform.GetChild(0).gameObject;
        textChar.text = c;
        Vector2 screenPos = Holder.instance.mainCamera.WorldToScreenPoint(worldPosition);
        this._rectTransform.position = screenPos;
        //Debug.Log("WorldPos = " + worldPosition + " screenPos = " + screenPos);
    }


    public void getFinish()
    {
        //Debug.Log("Sucess "+Time.timeSinceLevelLoad);
        _anima.SetTrigger("success");
    }

    public void launchFail()
    {
        _anima.SetTrigger("fail");
    }

    //getCall by the animation ! 
    public void endAnimation()
    {
        this.gameObject.SetActive(false);
    }
}
