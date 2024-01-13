using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class CutPlace : MonoBehaviour
{
    RectTransform parentRect;
    RectTransform rectTransform;
    MainVideoScript mainVideoScript;
    private bool entered=false;
    public bool isCut = false;

    void onEnter(){
        entered = true;
        MouseControl.instance.CutCursor();
    }
    void onLeave(){
        entered = false;
        MouseControl.instance.Default();
    }
    void Start(){
        parentRect = transform.parent.GetComponent<RectTransform>();
        rectTransform = GetComponent<RectTransform>();
        mainVideoScript = parentRect.GetComponent<MainVideoScript>();
    }
    void Update(){
        if (isCut) return;
        if (Input.GetMouseButtonDown(0) && entered){
            onLeave();
            gameObject.GetComponent<Image>().color = Color.blue;
            var i = mainVideoScript.getCut(this);
            if (i == null) return;
            int neighborindex = 0;
            if (i%2==0){
                neighborindex = i.Value+1;
            }else{
                neighborindex = i.Value-1;
            }
            
            var neighbor = mainVideoScript.cutPlaces[neighborindex];
            neighbor.GetComponent<Image>().color = Color.red;
            isCut = true;
            if (neighbor.isCut){
                gameObject.GetComponent<Image>().color = Color.gray;
                neighbor.GetComponent<Image>().color = Color.gray;
                mainVideoScript.CreateDeleteFragment(this,neighbor);
                Destroy(gameObject);
                Destroy(neighbor.gameObject);
            }
            return;
        }
        Vector2 point;
        var isvalid = RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRect, Input.mousePosition, Camera.main, out point);
        var mag = Vector2.Distance(point,(Vector2) (rectTransform.anchoredPosition + new Vector2(-15,rectTransform.rect.height/2)));
        if (mag<20 && !entered){
            onEnter();
        }
        if(mag >=20 && entered){
            onLeave();
        }
    }
}
