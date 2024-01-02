using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraWindow : WindowUI
{
    GameObject cameraDisplay;

    protected override void Start(){
        base.Start();
        cameraDisplay = transform.Find("Display").gameObject;
    }

    void Update(){
        cameraDisplay.SetActive(appOnBarReference.isFocused);
    }
}
