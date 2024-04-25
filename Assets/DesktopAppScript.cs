using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesktopAppScript : MonoBehaviour
{
    [SerializeField] protected AppObject app;
    protected DesktopHandler desktopHandler;
    float lastPress;

    protected virtual void openApp(){
        desktopHandler.openApp(app);
    }
    
    public void onPress(){
        if (Time.time-lastPress< 0.2f){
            openApp();
        }
        lastPress = Time.time;
    }

    private void Start(){
        desktopHandler = GameObject.Find("LaptopCanvas/Laptop/Desktop/DesktopContainer").GetComponent<DesktopHandler>();
    }
}
