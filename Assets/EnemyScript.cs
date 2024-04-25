using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public int aiLevel = 10;
    public float secondsToTryMove = 4;

    public float moveTime;

    public Action moveSucessCallback;
    public Action moveTryTaken;

    protected bool getChance(){
        var randValue = UnityEngine.Random.Range(0,20);
        if (aiLevel >= randValue){
            return true;
        }
        return false;
    }

    public void TryMove(){
        if (moveTryTaken != null)
            moveTryTaken();
        if (!getChance()) return;
        if (moveSucessCallback != null)
            moveSucessCallback();
    }
}
