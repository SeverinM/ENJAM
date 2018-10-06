using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DataInput {

    public KeyCode kc;
    public ToucheFX tFX;
    public Vector2 wPos;

    public DataInput(KeyCode keyCod, Vector2 worldPos)
    {
        kc = keyCod;
        wPos = worldPos;
    }
}
