using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DesktopHandler : MonoBehaviour
{
    
    [SerializeField] GameObject appPrefab;
    public Transform appsBarTransform;
    AppOnBar focusedApp;

    public void unfocus(){
        if (focusedApp == null) return;
        focusedApp.isFocused = false;
        focusedApp = null;
    }

    public void setFocus(AppOnBar apptoFocus){
        if(focusedApp != null){
            focusedApp.isFocused = false;
        }
        focusedApp = apptoFocus;
        focusedApp.isFocused = true;
        focusedApp.windowReference.gameObject.SetActive(true);
        focusedApp.windowReference.transform.SetAsLastSibling();
    }
    public void displayWindow(AppOnBar appThing){
        var window = Instantiate<WindowUI>(appThing.appReference.windowTemplate,transform,false);
        window.transform.localPosition = Vector3.zero;
        appThing.windowReference = window;
        window.appOnBarReference = appThing;
        setFocus(appThing);
    }

    public void onAppBarClick(AppOnBar appClicked){
        if (!appClicked.isFocused){
            setFocus(appClicked);
            return;
        }
        appClicked.windowReference.minimize();
    }

    public void openApp(AppObject app){
        var clone = Instantiate<GameObject>(appPrefab);
        clone.transform.SetParent(appsBarTransform,false);
        clone.name = app.name;
        var appbar = clone.GetComponent<AppOnBar>();
        appbar.appReference = app;
        clone.GetComponent<Button>().onClick.AddListener(()=>{
            onAppBarClick(appbar);
        });
        displayWindow(appbar);
        clone.transform.Find("Image").GetComponent<Image>().sprite = app.appImage;
    }
}
