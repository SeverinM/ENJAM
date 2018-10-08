using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class generator : MonoBehaviour {

    [SerializeField]
    List<GameObject> nuitSemaine;

    [SerializeField]
    List<GameObject> jourSemaine;

    [SerializeField]
    List<GameObject> nuitWE;

    [SerializeField]
    List<GameObject> jourWE;

    [SerializeField]
    int nombre = 2;

	// Use this for initialization
	void Start () {
        Holder hold = Holder.instance;
        
        for (int i = 0; i < nombre; i++)
        {
            hold.allScenette.Enqueue(randomChoice(jourWE));
            hold.allScenette.Enqueue(randomChoice(nuitWE));

            hold.allScenette.Enqueue(randomChoice(jourWE));
            hold.allScenette.Enqueue(randomChoice(nuitWE));

            hold.allScenette.Enqueue(randomChoice(jourSemaine));
            hold.allScenette.Enqueue(randomChoice(jourSemaine));
            hold.allScenette.Enqueue(randomChoice(nuitSemaine));


            hold.allScenette.Enqueue(randomChoice(jourSemaine));
            hold.allScenette.Enqueue(randomChoice(jourSemaine));

            hold.allScenette.Enqueue(randomChoice(nuitSemaine));
            hold.allScenette.Enqueue(randomChoice(jourSemaine));

            hold.allScenette.Enqueue(randomChoice(jourSemaine));
            hold.allScenette.Enqueue(randomChoice(jourSemaine));
            hold.allScenette.Enqueue(randomChoice(nuitSemaine));

            hold.allScenette.Enqueue(randomChoice(jourSemaine));
            hold.allScenette.Enqueue(randomChoice(nuitSemaine));

        }
	}

    public GameObject randomChoice(List<GameObject> gob)
    {
        int rando = Random.Range(0, gob.Count);
        print(rando + " / "+ gob.Count);
        return gob[rando];
    }
}
