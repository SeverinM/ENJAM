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
    [SerializeField]
    GameObject gobHonor2;
    RectTransform rectHonor;
    float yDiffHonor;
    public Gradient grd;


    [Header("Gestion des scenettes")]
    public List<GameObject> scenetteListTemp = new List<GameObject>();
    public Queue<GameObject> allScenette;
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

    [Header("Ecran secoué")]
    public float shake = 1;
    public float decreaseFactor = 1;


    private void Start()
    {
        audio = GameObject.FindObjectOfType<AudioSource>();
        allScenette = new Queue<GameObject>(scenetteListTemp);


        currentScenette = GameObject.Find("ScenetteTest").GetComponent<Scenette>();
        currentScenette.Fail += fail;
        currentScenette.Sucess += succ;
        setSpeed(currentScenette.speedBandeauMultipler);

        gobTimer = Instantiate(prefabTimer, this.transform) as GameObject;
        gobHonor = Instantiate(prefabHonor, transform);

        rect = gobTimer.GetComponent<RectTransform>();
        rectHonor = gobHonor.transform.GetComponent<RectTransform>();

        xDiff = rect.anchorMax.x - rect.anchorMin.x;
        yDiffHonor = rectHonor.anchorMax.y - rectHonor.anchorMin.y;

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
        currentScenette.startSequenceSequence();
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
            currentScenette.init();
        }

        if (allScenette.Count > 0)
        {
            obj = allScenette.Dequeue();
            obj.SetActive(true);
            currentScenette = Instantiate(obj).GetComponent<Scenette>();
            currentScenette.Sucess += succ;
            currentScenette.Fail += fail;
            currentScenette.GetComponent<Animator>().speed = currentScenette.mutliplerSpeedEnter;
            setSpeed(currentScenette.speedBandeauMultipler);
        }
       

        if (allScenette.Count > 0)
        {
            currentScenette.GetComponent<Animator>().SetFloat("speed", allScenette.ToArray()[0].GetComponent<Scenette>().mutliplerSpeedEnter);
        }

        currentScenette.init();
    }


    //Fin trnaisition
    public void activate()
    {
        Debug.Log("did I ever get called ?");
        gobHonor.SetActive(true);
        gobHonor2.SetActive(true);
        if (destroyingScenette != null)
            Destroy(destroyingScenette.gameObject);
        Destroy(gobBandeau);
        //currentScenette.enabled = true;
    }

    public void succ()
    {
        gobHonor.SetActive(false);
        gobTimer.SetActive(false);
        gobHonor2.SetActive(false);
        gobBandeau = Instantiate(prefabBandeau);
        gobBandeau.GetComponent<Bandeaux>().init(false, false,currentScenette.textVictory, currentScenette.textSizeVictory);
        StartCoroutine(nextSceneCoroutine());
    }

    public void fail()
    {
        gobHonor.SetActive(false);
        gobHonor2.SetActive(false);
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
            rectHonor.anchorMax = new Vector2(rectHonor.anchorMax.x,rectHonor.anchorMin.y + ((honor / honorMax) * yDiffHonor));
            gobHonor.GetComponent<UnityEngine.UI.Image>().color = grd.Evaluate(honor / honorMax);
        }

        if (shake > 0)
        {
            Camera.main.transform.localPosition = Random.insideUnitSphere * shake;
            shake -= Time.deltaTime * decreaseFactor;
        }
        else
        {
            shake = 0.0f;
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
