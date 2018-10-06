using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour {

    Scenette currentScenette;
    List<Scenette> allScenette = new List<Scenette>();

    public List<Scenette> GetScenettes()
    {
        return allScenette;
    }

	// Use this for initialization
	void Start () {
      
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void startNewScene(Scenette scn)
    {
    }

    void endScene(Scenette scn)
    {

    }
}
