using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum animHero
{
    attaqueAuDoigt,
    attaqueMoustique,
    dormirVener,
    pigeon,
    WEdormir,
    WEsquat,
    WEfooting,
    not
}

public class Perso : MonoBehaviour {

    public Animator animatorHero;

    public void setAnimator(animHero aH)
    {
        //Debug.LogWarning("I  have been called by "+this.transform.parent.name);
        animatorHero.SetTrigger("quit");
        animatorHero.SetTrigger(aH.ToString());
    }

    public void startEnergitize()
    {
        animatorHero.SetTrigger("go");
    }

}
