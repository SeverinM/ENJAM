using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour {

    Scenette currentScenette;
    Queue<Scenette> allScenette = new Queue<Scenette>();

    public Queue<Scenette> GetScenettes()
    {
        return allScenette;
    }

	// Use this for initialization
	void Start () {
        Holder holder = Holder.getInstance();
        holder.getKeyFx(KeyCode.A, new Vector2(0.5f, 0.5f));
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void startNewScene(Scenette scn)
    {
        currentScenette = allScenette.Dequeue();
    }

    void endScene(Scenette scn)
    {

    }
}
