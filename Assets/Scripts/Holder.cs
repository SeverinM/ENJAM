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
    [SerializeField]
    GameObject gobHonor;
    RectTransform rectHonor;
    float yDiffHonor;
    public Gradient grd;


    [Header("Gestion des scenettes")]
    public List<GameObject> scenetteListTemp = new List<GameObject>();
    public Queue<GameObject> allScenette;
    public GameObject victoire;
    public GameObject defaite;
    Scenette currentScenette;
    Scenette destroyingScenette;
    public GameObject moustiqueGO;

    [Header("Bandeau")]
    public GameObject prefabBandeau;
    GameObject gobBandeau;
    float speedBandeau = 1;

    AudioSource audio;
    [Header("Son")]
    public AudioClip successClic;
    public AudioClip failClic;
    public List<AudioClip> soundsSuccess;
    public List<AudioClip> soundsFail;
    public List<AudioClip> next;
    public AudioClip failSound;

    [Header("Ecran secoué")]
    public float shake = 1;
    public float decreaseFactor = 1;
    public float minimum = 0;

    bool isWE;

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
        rectHonor = gobHonor.transform.GetChild(0).GetComponent<RectTransform>();

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
        audio.PlayOneShot(clp, 0.5f);
    }

    public float getSpeed()
    {
        return speedBandeau;
    }

    //Fin bandeau , debut transition
    public void nextScene()
    {
        audio.PlayOneShot(next[Random.Range(0, next.Count)], 0.5f);
        GameObject obj = null;

        if(honor < 0)
        {
            allScenette.Clear();
            allScenette.Enqueue(defaite);
        }
        else if(honor > honorMax)
        {
            allScenette.Clear();
            allScenette.Enqueue(victoire);
        }

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

        isWE = currentScenette.isWE;
        gobTimer.SetActive(!currentScenette.falseScene);
        currentScenette.init();
    }


    //Fin trnaisition
    public void activate()
    {
        if (destroyingScenette != null)
            Destroy(destroyingScenette.gameObject);
        Destroy(gobBandeau);
    }

    public void succ()
    {
        gobTimer.SetActive(false);
        gobBandeau = Instantiate(prefabBandeau);
        gobBandeau.GetComponent<Bandeaux>().init(isWE, false,currentScenette.textVictory, currentScenette.textSizeVictory);
        StartCoroutine(nextSceneCoroutine());
        minimum = shake = 0;
    }

    public void fail()
    {
        gobTimer.SetActive(false);
        gobBandeau = Instantiate(prefabBandeau);
        gobBandeau.GetComponent<Bandeaux>().init(isWE, true,currentScenette.textDefeat, currentScenette.textSizeDefeat);
        StartCoroutine(nextSceneCoroutine());
        audio.PlayOneShot(failSound, 0.5f);
        minimum = shake = 0;
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

        if (shake > minimum)
        {
            Vector2 rand = Random.insideUnitCircle * shake;
            Camera.main.transform.localPosition = new Vector3(rand.x, rand.y, -10);
            shake -= Time.deltaTime * decreaseFactor;
        }
        else
        {
            shake = minimum;
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
