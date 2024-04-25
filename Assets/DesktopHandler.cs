using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DesktopHandler : MonoBehaviour
{
    
    [SerializeField] Transform defaultPopup;
    [SerializeField] GameObject appPrefab;
    [SerializeField] AppObject popupPrefab;
    public DragableFile currentDrag;
    public Transform appsBarTransform;
    AppOnBar focusedApp;

    public static DesktopHandler instance;

    void Start(){
        instance = this;
    }

    public InfoboxScript createPopup(Transform inside = null){
        if (inside == null) {
            inside = defaultPopup;
        }
        var window = openApp(popupPrefab);
        var InfoboxScript = window.GetComponent<InfoboxScript>();
        InfoboxScript.SetDisplay(inside);
        var a = window.transform.Find("ExitButton");
        if (a != null){
            print(a);
            a.GetComponent<Button>().onClick.AddListener(() => {
                window.close();
            });
        }
        return InfoboxScript;
    }

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
    public WindowUI displayWindow(AppOnBar appThing){
        var window = Instantiate<WindowUI>(appThing.appReference.windowTemplate,transform,false);
        window.transform.localPosition = Vector3.zero;
        appThing.windowReference = window;
        window.appOnBarReference = appThing;
        setFocus(appThing);
        return window;
    }

    public void onAppBarClick(AppOnBar appClicked){
        if (!appClicked.isFocused){
            setFocus(appClicked);
            return;
        }
        appClicked.windowReference.minimize();
    }

    public WindowUI openApp(AppObject app){
        var clone = Instantiate<GameObject>(appPrefab);
        clone.transform.SetParent(appsBarTransform,false);
        clone.name = app.name;
        var appbar = clone.GetComponent<AppOnBar>();
        appbar.appReference = app;
        clone.GetComponent<Button>().onClick.AddListener(()=>{
            onAppBarClick(appbar);
        });
        var window = displayWindow(appbar);
        clone.transform.Find("Image").GetComponent<Image>().sprite = app.appImage;
        return window;
    }
    void Update(){
        if (GameManager.instance.gameState == GameState.End){
            transform.parent.parent.gameObject.SetActive(false);
        }
    }
}
