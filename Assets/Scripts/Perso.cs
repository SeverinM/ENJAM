﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum animHero
{
    attaqueAuDoigt,
    attaqueMoustique,
    dormirVener,
    WEdormir,
    WEsquat
}

public class Perso : MonoBehaviour {

    public Animator animatorHero;

    public void setAnimator(animHero aH)
    {
        animatorHero.SetTrigger("quit");
        animatorHero.SetTrigger(aH.ToString());
    }

    public void startEnergitize()
    {
        animatorHero.SetTrigger("go");
    }

}