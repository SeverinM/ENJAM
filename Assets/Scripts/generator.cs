using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class generator : MonoBehaviour {

    [SerializeField]
    List<GameObject> nuitWE;

    [SerializeField]
    List<GameObject> jourWE;

    [SerializeField]
    List<GameObject> nuitSemaine;

    [SerializeField]
    List<GameObject> jourSemaine;

    [SerializeField]
    int nombre = 10;
    bool isWE = false;
    bool isNight = false;

	// Use this for initialization
	void Start () {
        Holder hold = Holder.instance;
        int countWeek = 0;
        for (int i = 0; i < nombre; i++, countWeek++)
        {
            if (isWE)
            {
                if(isNight)
                {
                    hold.allScenette.Enqueue(nuitWE[Random.Range(0,nuitWE.Count)]);
                }
                else
                {
                    hold.allScenette.Enqueue(jourWE[Random.Range(0, jourWE.Count)]);
                }
            }
            else
            {
                if (isNight)
                {
                    hold.allScenette.Enqueue(nuitSemaine[Random.Range(0, nuitSemaine.Count)]);
                }
                else
                {
                    hold.allScenette.Enqueue(jourSemaine[Random.Range(0, jourSemaine.Count)]);
                }
            }

            if (countWeek == 4 && isWE)
            {
                countWeek = 0;
                isWE = !isWE;
            }

            if (countWeek == 5 && !isWE)
            {
                countWeek = 0;
                isWE = !isWE;
            }

            isNight = !isNight;
        }
	}
}
