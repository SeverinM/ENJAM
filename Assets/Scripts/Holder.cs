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

    public GameObject prefabInstanceToucheFX;
    GameObject[] poolKey = new GameObject[SIZEPOOLTOUCHE];

    public Camera mainCamera;

    [SerializeField]
    float honor;
    float honorMax = 100;
    public GameObject prefabHonor;
    GameObject gobHonor;
    RectTransform rectHonor;
    float xDiffHonor;

    public List<GameObject> scenetteListTemp = new List<GameObject>();
    Queue<GameObject> allScenette;
    Scenette currentScenette;
    Scenette destroyingScenette;

    public GameObject prefabBandeauWin;
    public GameObject prefabBandeauLose;
    GameObject gobBandeau;
    float speedBandeau = 1;

    private void Start()
    {
        allScenette = new Queue<GameObject>(scenetteListTemp);
        //DEBUG, NEED TO BE ERASE AFTER
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

    public void setSpeed(float speed)
    {
        speedBandeau = speed;
    }

    public float getSpeed()
    {
        return speedBandeau;
    }

    public void nextScene()
    {
        if (allScenette.Count == 0)
        {
            return;
        }
        if (currentScenette != null)
        {
            currentScenette.Sucess -= succ;
            currentScenette.Fail -= fail;
            destroyingScenette = currentScenette;
        }

        GameObject obj = allScenette.Dequeue();
        obj.SetActive(true);
        currentScenette = Instantiate(obj).GetComponent<Scenette>();
        currentScenette.GetComponent<Animator>().speed = currentScenette.mutliplerSpeedEnter;

        currentScenette.Sucess += succ;
        currentScenette.Fail += succ;
    }

    public void activate()
    {
        gobHonor.SetActive(true);
        gobHonor.SetActive(true);
        Destroy(destroyingScenette.gameObject);
        Destroy(gobBandeau);
        currentScenette.enabled = true;
        currentScenette.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Default";
    }

    public void succ()
    {
        gobHonor.SetActive(false);
        gobTimer.SetActive(false);
        gobBandeau = Instantiate(prefabBandeauWin);
        StartCoroutine(nextSceneCoroutine());
    }

    public void fail()
    {
        gobHonor.SetActive(false);
        gobTimer.SetActive(false);
        gobBandeau = Instantiate(prefabBandeauLose);
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

    public void setHonor(float newHonor)
    {
        honor = newHonor;
    }

    IEnumerator nextSceneCoroutine()
    {
        yield return new WaitForSeconds(currentScenette.timeBeforeNext);
        nextScene();
        yield return 0;
    }
}
