using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Holder : MonoBehaviour {


    public float Vitesse = 0;
    public static Holder instance;
    public GameObject prefabInstanceToucheFX;
    Dictionary<KeyCode, GameObject> poolKey = new Dictionary<KeyCode, GameObject>();

    public Camera mainCamera;

    private void Start()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
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
            output.transform.parent = FindObjectOfType<Canvas>().transform;
        }
        output.GetComponent<ToucheFX>().init(worldPosition, kc.ToString());
        output.SetActive(true);
        return output;
    }
}
