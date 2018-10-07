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

        _anima.speed = Random.Range(0.5f,1.7f);
    }

    public void init(Vector2 worldPosition, string c )
    {
        GameObject child = transform.GetChild(0).gameObject;
        textChar.text = c;

        /*ector3 positionScreen = Holder.instance.mainCamera.WorldToScreenPoint(worldPosition);
         Vector2 screenPos = new Vector2(positionScreen.x / Screen.width, positionScreen.y / Screen.height);*/
        _rectTransform.anchorMax = worldPosition;
        _rectTransform.anchorMin = worldPosition;
        _rectTransform.anchoredPosition = new Vector3(0, 0, 0);
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
