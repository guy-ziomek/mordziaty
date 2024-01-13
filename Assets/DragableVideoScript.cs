using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Video;

public class DragableVideoScript : MonoBehaviour, IEndDragHandler, IBeginDragHandler, IDragHandler, IPointerDownHandler
{
    private RectTransform rectTransform;
    CanvasGroup canvasGroup;
    private Vector3 MouseDragStartPos;
    public VideoClip videoClip;
    private RectTransform canvasRect;

    private double startTime;
    private double endTime;

     public double getTime(double time){
        var newTIme = time - startTime;
        print($"{newTIme} CLIP THING");
        return newTIme;
    }

    void Start(){
        rectTransform = GetComponent<RectTransform>();
        canvasRect = transform.parent.GetComponent<RectTransform>();
        canvasGroup = transform.Find("RedLight").GetComponent<CanvasGroup>();
    }

    private bool checkOverlap(RectTransform rect1, RectTransform rect2){
        var minx1 = (rect1.localPosition-new Vector3(rect1.rect.width/2,0)).x;
        var maxx1 = (rect1.localPosition+new Vector3(rect1.rect.width/2,0)).x;
        var minx2 = (rect2.localPosition-new Vector3(rect2.rect.width/2,0)).x;
        var maxx2 = (rect2.localPosition+new Vector3(rect2.rect.width/2,0)).x;
        if (maxx1 < minx2 || minx1 > maxx2){
            return false;
        }
        return true;
    }

    public bool canBePlaced(){
        if (canvasRect == null) return false;
        foreach(RectTransform rect in canvasRect){
            if (rect == rectTransform) continue;
            var p = checkOverlap(rectTransform, rect);
            if (p) return false;
        }
        return true;
    }
    private Vector2 oldPosition;

    public void OnBeginDrag(PointerEventData eventData)
    {
         if (eventData.button != PointerEventData.InputButton.Left) return; 
        Vector2 point;
        var isvalid = RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, Input.mousePosition, Camera.main, out point);
        if (isvalid)
            oldPosition = rectTransform.anchoredPosition;
            MouseDragStartPos = point-rectTransform.anchoredPosition;
            transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
         Vector2 point;
            var isvalid = RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, Input.mousePosition, Camera.main, out point);
            if (isvalid){
                var roundedX = Mathf.Round(((Vector3)point-MouseDragStartPos).x/8)*8;
                rectTransform.anchoredPosition = new Vector2(roundedX, rectTransform.anchoredPosition.y);
                if (canBePlaced()){
                    canvasGroup.alpha = 0;
                }
                else{
                    canvasGroup.alpha = 0.6f;
                }
            }
    }

    public void updateTimes(){
        var width = rectTransform.rect.width;
        startTime = (double) (rectTransform.anchoredPosition.x-width/2)/100;
        endTime = (double) (rectTransform.anchoredPosition.x+width/2)/100;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 0;
        if (!canBePlaced()){
            rectTransform.localPosition = oldPosition;
        }
        updateTimes();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right){
            Destroy(gameObject);
        }
    }

    void Update(){
        var curTime = EditingWindow.instance.currentTime ;
        if (curTime <= endTime && curTime > startTime){
            EditingWindow.instance.currentSecondVideo = this;
        }else if (EditingWindow.instance.currentSecondVideo == this){
            EditingWindow.instance.currentSecondVideo = null;
        }
    }
}
