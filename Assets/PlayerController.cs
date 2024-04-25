using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    float currentRotation = 0;
    bool canRotate = true;
    
    Dictionary<int,Action> roomFunctions = new Dictionary<int,Action> {
        {0, () =>{
            print("Nic!");
        }},
        {180, ()=>{
            print("Byk");
        }}
    };

    void Start()
    {
        roomFunctions.Add(90,swipeKomp);
        roomFunctions.Add(270,()=>{
            canRotate = !canRotate;
            if (!canRotate){
                StartCoroutine(moveCamera(Vector3.forward*10f,2));
            }else{
                StartCoroutine(moveCamera(Vector3.forward*-10f,2));
            } 
        });
    }

    IEnumerator moveCamera(Vector3 vector, float timetotake){
        if (cameraMoving) yield break;
        var cam = Camera.main.transform;
        var startPos = cam.position;
        var dest = startPos+cam.TransformDirection(vector);
        float timer = 0;
        cameraMoving = true;
        while (timer < 1){
            timer+=Time.deltaTime/timetotake;
            cam.position = Vector3.Lerp(startPos, dest,timer);
            yield return null;
        }
        cameraMoving = false;
        yield break;
    }

    void swipeKomp(){
        canRotate = !canRotate;
        if (!canRotate){
            StartCoroutine(moveCamera(Vector3.forward*3.5f,0.25f));
        }else{
            StartCoroutine(moveCamera(Vector3.forward*-3.5f,0.25f));
        }
    }

    bool cameraMoving = false;
    IEnumerator rotateCamera(int dir){
        if (cameraMoving) yield break;
        var totalMoveangle = 0f;
        cameraMoving = true;
        var startRotation = Camera.main.transform.rotation;
        while (totalMoveangle <90)
        {
            totalMoveangle += 200*Time.deltaTime;
            Camera.main.transform.Rotate(Vector3.up*200*dir*Time.deltaTime);
            yield return null;
        }
        currentRotation += 90*dir;
        currentRotation%=360;
        if (currentRotation < 0) currentRotation=360+currentRotation;
        Camera.main.transform.rotation = startRotation*Quaternion.Euler(Vector3.up*90*dir);
        cameraMoving = false;
        yield break;
    }

    bool downDebounce = false;

    void Update()
    {
        if (cameraMoving) return;
        if (Input.GetMouseButton(0)) return;
        var mPx = Input.mousePosition.x;
        var mPy = Input.mousePosition.y;
        if (mPx < 80 && canRotate){
            StartCoroutine(rotateCamera(-1));
            return;
        }else if(mPx > 850 && canRotate){
            StartCoroutine(rotateCamera(1));
            return;
        }
        if (mPy < 70){
            if (downDebounce) return;
            downDebounce = true;
            roomFunctions[(int) currentRotation]();
        }else{
            downDebounce = false;
        }
    }
}
