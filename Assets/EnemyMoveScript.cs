using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MovePosition{
    public Vector3 position;
    public string positionName = "position";
    [SerializeField] protected MovePosition parentPosition;

    public int parentId = 0;

    public List<int> childrenIds;
}

public class EnemyMoveScript : MonoBehaviour
{
    public MovePosition currentPosition;
    public MovePosition[] positions;

    public System.Action handleIndexOverLength;

    private MovePosition startNode;

    public bool goNextPos(int step = 1){
        
        var lastNode = currentPosition.childrenIds.Count <= 0;
        if (lastNode){
            handleIndexOverLength(); //made it a method if someone for example would backtrack a couple of tiles instead of a full loop
            return false;
        }

        for (int i = 0; i < step; i++){
            currentPosition = positions[currentPosition.childrenIds[
                UnityEngine.Random.Range(0,currentPosition.childrenIds.Count) // random next position
            ]];
        }
        
        transform.position = currentPosition.position;
        return currentPosition.childrenIds.Count <= 0;
    } //returns true if is at the end
    public void setNextPos(int ind){
        currentPosition = positions[ind];
        goNextPos(0);
    }
    void Start(){

        foreach(var position in positions){
            position.childrenIds = new List<int>();
        }

        int i = 0;
        foreach(var position in positions){
            if (positions[position.parentId]==position){
                startNode = position;
            }else{
                positions[position.parentId].childrenIds.Add(i);
            }
            i++;
        }
        currentPosition = startNode;
        handleIndexOverLength = () =>{
            currentPosition = startNode;
        };
        goNextPos(0);
    }

    
    public void SetSelectedPosition(int index){
        positions[index].position = transform.position;
    }
    public void RevertSelectedPosition(int index){
        transform.position = positions[index].position;
    }

}
