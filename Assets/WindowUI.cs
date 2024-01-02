using System.Collections;
using System.Collections.Generic;
using Mono.Cecil.Cil;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class WindowUI : MonoBehaviour, IDragHandler, IPointerDownHandler
{
    protected RectTransform rectTransform;
    protected RectTransform desktopRect;
    Vector3 MouseDragStartPos;
    public AppOnBar appOnBarReference;
    protected DesktopHandler desktopHandler;
    float borderSnapSize = 1;

    public void OnDrag(PointerEventData eventData)
    {
        if (maxed) return;
        if (eventData.button != PointerEventData.InputButton.Left) return; 
            Vector2 point;
            var isvalid = RectTransformUtility.ScreenPointToLocalPointInRectangle(desktopRect, Input.mousePosition, Camera.main, out point);
            if (isvalid){
                var oldpos = transform.localPosition;
                transform.localPosition = (Vector3) point-MouseDragStartPos;
                Vector2 anchoredPosition = rectTransform.anchoredPosition;
                print(anchoredPosition);
                if(anchoredPosition.x+rectTransform.rect.width/2>desktopRect.rect.width/2){
                    anchoredPosition.x=desktopRect.rect.width/2-rectTransform.rect.width/2;
                }
                if(anchoredPosition.x-rectTransform.rect.width/2<-desktopRect.rect.width/2){
                    anchoredPosition.x=-desktopRect.rect.width/2+rectTransform.rect.width/2;
                }
                if(anchoredPosition.y+rectTransform.rect.height/2>desktopRect.rect.height/2){
                    anchoredPosition.y=desktopRect.rect.height/2-rectTransform.rect.height/2;
                }
                if(anchoredPosition.y-rectTransform.rect.height/2<-desktopRect.rect.height/2){
                    anchoredPosition.y=-desktopRect.rect.height/2+rectTransform.rect.height/2;
                }
                rectTransform.anchoredPosition = anchoredPosition;

            }
            
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return; 
        Vector2 point;
        var isvalid = RectTransformUtility.ScreenPointToLocalPointInRectangle(desktopRect, Input.mousePosition, Camera.main, out point);
        if (isvalid)
            MouseDragStartPos = (Vector3) point-transform.localPosition;
        focusWindow();
    }

    public void focusWindow(){
        desktopHandler.setFocus(appOnBarReference);
    }

    bool maxed = false;

    Vector2 oldsize;
    Vector2 oldpos; 

    private void upscale(){
        maxed = true;
        var parentRect = (RectTransform) transform.parent;
        oldsize = rectTransform.sizeDelta;
        oldpos = rectTransform.localPosition;
        rectTransform.sizeDelta=new Vector2(parentRect.rect.width,parentRect.rect.height);
        rectTransform.localPosition = new Vector2(0,0);
        focusWindow();
    }
    private void downscale(){
        maxed = false;
        rectTransform.sizeDelta = oldsize;
        rectTransform.localPosition=oldpos;
        focusWindow();
    }

    public void close(){
        Destroy(appOnBarReference.gameObject);
        Destroy(gameObject);
    }

    public void minimize(){
        transform.gameObject.SetActive(false);
        desktopHandler.unfocus();
    }

    public void upscaleDownscale(){
        if (!maxed) upscale();
        else if (maxed) downscale();
    }

    protected virtual void Start()
    {
        rectTransform = (RectTransform) transform;
        desktopHandler = GameObject.Find("LaptopCanvas/Desktop/DesktopContainer").GetComponent<DesktopHandler>();
        desktopRect = (RectTransform) rectTransform.parent.parent;
    }

    void Update()
    {
    }

}
