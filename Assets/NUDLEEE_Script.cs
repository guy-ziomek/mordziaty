using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NUDLEEE_Script : MonoBehaviour
{
    EnemyScript enemyScript;
    EnemyMoveScript moveScript;
    bool isWaitingNearPlayer = false;
    int timer = 0;
    void OnSucessfulMove(){
        if (isWaitingNearPlayer){
            enemyScript.moveTryTaken = () =>{
                timer += 1;
            };
            return;
        }
        if (moveScript.goNextPos()){
            print("O_O");
            isWaitingNearPlayer = true;
        };
    }




    void Start()
    {
        enemyScript = GetComponent<EnemyScript>();
        moveScript = GetComponent<EnemyMoveScript>();
        enemyScript.moveSucessCallback += OnSucessfulMove;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)){
            timer = 0;
            enemyScript.moveTryTaken = null;
            moveScript.setNextPos(0);
            isWaitingNearPlayer = false;
        }
        if (timer >= 2){
            GameManager.instance.GameOver("nudle killa");
        }
    }
}
