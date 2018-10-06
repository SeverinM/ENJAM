using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Holder : MonoBehaviour {


    public float Vitesse = 0;
    public static Holder instance;

    private float maxTime;
    private float actualTime;

    RectTransform rect;
    public GameObject prefabTimer;
    GameObject gobTimer;
    float xDiff;

    public GameObject prefabInstanceToucheFX;
    Dictionary<KeyCode, GameObject> poolKey = new Dictionary<KeyCode, GameObject>();

    [SerializeField]
    float honor;
    float honorMax = 100;
    public GameObject prefabHonor;
    GameObject gobHonor;
    RectTransform rectHonor;
    float xDiffHonor;

    private void Start()
    {
        gobTimer = Instantiate(prefabTimer, this.transform) as GameObject;
        gobHonor = Instantiate(prefabHonor, transform);

        rect = gobTimer.GetComponent<RectTransform>();
        rectHonor = gobHonor.GetComponent<RectTransform>();

        xDiff = rect.anchorMax.x - rect.anchorMin.x;
        xDiffHonor = rectHonor.anchorMax.x - rectHonor.anchorMin.x;

        setTime(5);
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
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
        GameObject output;
        if (poolKey.ContainsKey(kc))
        {
            output = poolKey[kc];
        }
        else
        {
            output = poolKey[kc] = Instantiate(prefabInstanceToucheFX) as GameObject;
            output.transform.SetParent(transform);
        }
        output.GetComponent<ToucheFX>().init(worldPosition, kc.ToString());
        output.SetActive(true);
        return output;
    }

    public void setTime(float time)
    {
        actualTime = maxTime = time;
        gobTimer.SetActive(true);
    }

    public void setHonor(float newHonor)
    {
        honor = newHonor;
    }
}
