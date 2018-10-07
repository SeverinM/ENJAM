using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToucheFX : MonoBehaviour {

    [SerializeField]
    private Text textChar;

    private Animator _anima;
    private RectTransform _rectTransform;

    private Vector2 realPosition;
    private Vector3 offset = new Vector3(-3.4f,-4.1f,-4f);
    private Animator moustiqueLinked;
    

    public void Awake()
    {
        _anima = this.GetComponent<Animator>();
        _rectTransform = this.GetComponent<RectTransform>();

        _rectTransform.sizeDelta = new Vector2(Screen.height / 11, Screen.height / 11) ;

        _anima.speed = Random.Range(0.5f,1.7f);
    }

    public void init(Vector2 worldPosition, string c )
    {
        if (c.StartsWith("Alpha"))
        {
            c = c.Substring(c.Length - 1);
        }
        textChar.text = c;

        Vector3 positionScreen = Holder.instance.mainCamera.WorldToScreenPoint(worldPosition);
        _rectTransform.position = positionScreen;

        realPosition = worldPosition;
    }


    public void getFinish()
    {
        //Debug.Log("Sucess "+Time.timeSinceLevelLoad);
        _anima.SetTrigger("success");
        if (moustiqueLinked != null)
            moustiqueLinked.SetTrigger("death");
    }

    public void launchFail()
    {
        if (moustiqueLinked != null)
            Destroy(moustiqueLinked.gameObject);
        _anima.SetTrigger("fail");
    }

    //getCall by the animation ! 
    public void endAnimation()
    {
        this.gameObject.SetActive(false);
    }

    public void Stickmou()
    {
        print("Moustique !");
        moustiqueLinked = Instantiate(Holder.instance.moustiqueGO, (Vector3)realPosition + offset, Quaternion.identity).GetComponent<Animator>();
    }

}
