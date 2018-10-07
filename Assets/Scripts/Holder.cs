using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Holder : MonoBehaviour {

    public static uint SIZEPOOLTOUCHE = 50;

    public float Vitesse = 0;
    public static Holder instance;

    private float maxTime;
    private float actualTime;

    RectTransform rect;
    public GameObject prefabTimer;
    GameObject gobTimer;
    float xDiff;
    public GameObject txtRestant;

    public GameObject prefabInstanceToucheFX;
    GameObject[] poolKey = new GameObject[SIZEPOOLTOUCHE];

    public Camera mainCamera;


    [Header("Honneur")]
    [SerializeField]
    public float honor;
    float honorMax = 100;
    public GameObject prefabHonor;
    public float honorLosePerTouch;
    public float honorMaxPerWin;
    GameObject gobHonor;
    RectTransform rectHonor;
    float xDiffHonor;


    [Header("Gestion des scenettes")]
    public List<GameObject> scenetteListTemp = new List<GameObject>();
    Queue<GameObject> allScenette;
    Scenette currentScenette;
    Scenette destroyingScenette;

    [Header("Bandeau")]
    public GameObject prefabBandeau;
    GameObject gobBandeau;
    float speedBandeau = 1;

    [Header("Son")]
    AudioSource audio;
    public AudioClip successClic;
    public AudioClip failClic;
    public List<AudioClip> soundsSuccess;
    public List<AudioClip> soundsFail;

    private void Start()
    {
        audio = GameObject.FindObjectOfType<AudioSource>();
        allScenette = new Queue<GameObject>(scenetteListTemp);


        currentScenette = GameObject.Find("ScenetteTest").GetComponent<Scenette>();
        currentScenette.Fail += fail;
        currentScenette.Sucess += succ;

        gobTimer = Instantiate(prefabTimer, this.transform) as GameObject;
        gobHonor = Instantiate(prefabHonor, transform);

        rect = gobTimer.GetComponent<RectTransform>();
        rectHonor = gobHonor.GetComponent<RectTransform>();

        xDiff = rect.anchorMax.x - rect.anchorMin.x;
        xDiffHonor = rectHonor.anchorMax.x - rectHonor.anchorMin.x;

        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

        for (int index = 0; index < SIZEPOOLTOUCHE; index ++)
        {
            poolKey[index] = Instantiate(prefabInstanceToucheFX) as GameObject;
            poolKey[index].SetActive(false);
            poolKey[index].transform.SetParent(transform);
        }


        //Start Game  :
        currentScenette.init();
    }

    public void setText(int nb)
    {
        txtRestant.GetComponent<UnityEngine.UI.Text>().text = nb.ToString();
    }

    public void setSpeed(float speed)
    {
        speedBandeau = speed;
    }

    public void Play(AudioClip clp)
    {
        audio.PlayOneShot(clp);
    }

    public float getSpeed()
    {
        return speedBandeau;
    }

    //Fin bandeau , debut transition
    public void nextScene()
    {
        GameObject obj = null;
        if (currentScenette != null)
        {
            currentScenette.Sucess -= succ;
            currentScenette.Fail -= fail;
            destroyingScenette = currentScenette;
            currentScenette.Sucess += succ;
            currentScenette.Fail += succ;
        }

        if (allScenette.Count > 0)
        {
            obj = allScenette.Dequeue();
            obj.SetActive(true);
            currentScenette = Instantiate(obj).GetComponent<Scenette>();
            currentScenette.Sucess += succ;
            currentScenette.Fail += fail;
            currentScenette.GetComponent<Animator>().speed = currentScenette.mutliplerSpeedEnter;
        }
       

        if (allScenette.Count > 0)
        {
            currentScenette.GetComponent<Animator>().SetFloat("speed", allScenette.ToArray()[0].GetComponent<Scenette>().mutliplerSpeedEnter);
        }
    }


    //Fin trnaisition
    public void activate()
    {
        gobHonor.SetActive(true);
        Destroy(destroyingScenette.gameObject);
        Destroy(gobBandeau);
        currentScenette.enabled = true;
    }

    public void succ()
    {
        gobHonor.SetActive(false);
        gobTimer.SetActive(false);
        gobBandeau = Instantiate(prefabBandeau);
        gobBandeau.GetComponent<Bandeaux>().init(false, false,currentScenette.textVictory, currentScenette.textSizeVictory);
        StartCoroutine(nextSceneCoroutine());
    }

    public void fail()
    {
        gobHonor.SetActive(false);
        gobTimer.SetActive(false);
        gobBandeau = Instantiate(prefabBandeau);
        gobBandeau.GetComponent<Bandeaux>().init(false, true,currentScenette.textDefeat, currentScenette.textSizeDefeat);
        StartCoroutine(nextSceneCoroutine());
    }

    private void Update()
    {
        if (gobTimer.activeSelf)
        {
            actualTime -= (Time.deltaTime * Vitesse);
            rect.anchorMax = new Vector2(rect.anchorMin.x + ((actualTime / maxTime) * xDiff), rect.anchorMax.y);
            if (actualTime <= 0)
            {
                gobTimer.SetActive(false);
            }
        }

        if (gobHonor.activeSelf)
        {
            rectHonor.anchorMax = new Vector2(rectHonor.anchorMin.x + ((honor / honorMax) * xDiffHonor), rectHonor.anchorMax.y);
        }
    }

    public static Holder getInstance()
    {
        if (instance == null)
        {
            instance = new GameObject().AddComponent<Holder>();
        }
        return instance;
    }

    public GameObject getKeyFx(KeyCode kc, Vector2 worldPosition)
    {
        GameObject output = null;
        for (int index = 0; index < SIZEPOOLTOUCHE; index++)
        {
            if (!poolKey[index].activeSelf)
            {
                output = poolKey[index];
                break;
            }

        }
        if(output == null)
        {
            Debug.LogError("Pas le place ? Serieux ??? Y a plus de 100 touches actives a l'ecran ? Mais wat ?");
        }
        output.GetComponent<ToucheFX>().init(worldPosition, kc.ToString());
        output.SetActive(true);
        return output;
    }

    public void setTime(float time, bool restart = true)
    {
        actualTime = time;
        if (restart)
        {
            maxTime = time;
        }
        gobTimer.SetActive(true);
    }

    public void timerHurt()
    {
        gobTimer.GetComponent<Animator>().SetTrigger("hit"); 
    }


    IEnumerator nextSceneCoroutine()
    {
        yield return new WaitForSeconds(currentScenette.timeBeforeNext);
        nextScene();
        yield return 0;
    }

    public static Vector3 worldPointToRatio(Vector3 point)
    {
        Vector3 output;
        point = Camera.main.WorldToScreenPoint(point);
        output = new Vector3(point.x / Screen.width, point.y / Screen.height, 0);
        return output;
    }
}
