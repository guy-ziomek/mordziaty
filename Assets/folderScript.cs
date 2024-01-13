using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class folderScript : MonoBehaviour
{
    [SerializeField] Transform destination;

    float lastclick = 0;
    public void folderOpen(){
        if (Time.time-lastclick < 0.5f){
            foreach (Transform trans in transform.parent.parent){
                trans.gameObject.SetActive(false);
            }
            destination.gameObject.SetActive(true);
        }
        lastclick = Time.time;

    }  
}
