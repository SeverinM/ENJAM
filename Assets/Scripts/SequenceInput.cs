using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

[System.Serializable]
public class SequenceInput {

    public List<DataInput> dI = new List<DataInput>();

    public List<string> ToStringAlt()
    {
        List<string> txt = new List<string>();
        StringBuilder sb = new StringBuilder();
        txt.Add("=======");
        foreach(DataInput dIE in dI)
        {
            txt.Add("=======");
            txt.Add(dIE.kc + " - " + string.Format("( {0} / {1} )\n", dIE.wPos.x, dIE.wPos.y));
        }
        return txt;
    }
}
