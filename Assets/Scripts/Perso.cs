﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum animHero
{
    attaqueAuDoigt,
    attaqueMoustique,
    dormirVener,
    pigeon,
    phone,
    douche,
    allerAuTaf,
    escalierMontee,
    escalierDesc,
    toilettes,
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
        if (!Holder.instance.currentScenette.falseScene && !Holder.instance.currentScenette.isWE)
            Holder.instance.minimum = Holder.instance.shake = 0.1f;
    }

}
