using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class PhotoshopImage : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{

    public string fileName = "asd.png";
    protected RectTransform rectTransform;
    protected RectTransform desktopRect;
    Vector3 MouseDragStartPos;
    public void OnDrag(PointerEventData eventData)
    {
        if (PhotoshopWindow.instance.Mode != PhotoshopMode.Move) return;
        if (eventData.button != PointerEventData.InputButton.Left) return; 
            Vector2 point;
            var isvalid = RectTransformUtility.ScreenPointToLocalPointInRectangle(desktopRect, Input.mousePosition, Camera.main, out point);
            if (isvalid){
                var oldpos = transform.localPosition;
                transform.localPosition = (Vector3) point-MouseDragStartPos;
                Vector2 anchoredPosition = rectTransform.anchoredPosition;
                if(anchoredPosition.x>desktopRect.rect.width/2){
                    anchoredPosition.x=desktopRect.rect.width/2;
                }
                if(anchoredPosition.x<-desktopRect.rect.width/2){
                    anchoredPosition.x=-desktopRect.rect.width/2;
                }
                if(anchoredPosition.y>desktopRect.rect.height/2){
                    anchoredPosition.y=desktopRect.rect.height/2;
                }
                if(anchoredPosition.y<-desktopRect.rect.height/2){
                    anchoredPosition.y=-desktopRect.rect.height/2;
                }
                rectTransform.anchoredPosition = anchoredPosition;

            }
            
    }

    public ImageCondition checkConditions(){
        foreach (ImageCondition condition in transform.GetComponents<ImageCondition>()){
            if (!condition.checkValid()) return condition;
        }
        return null;
    }

    public void focusObject(){
        gameObject.transform.SetAsLastSibling();
    }

    private void Start()
    {
        rectTransform = (RectTransform) transform;
        desktopRect = (RectTransform) PhotoshopWindow.instance.transform.Find("Display/RightSide/Canvas");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;
        if (PhotoshopWindow.instance.Mode == PhotoshopMode.Color){
            Color color;
            var success = ColorUtility.TryParseHtmlString(PhotoshopWindow.instance.hexColorInput.text, out color);
            if (!success){
                PhotoshopWindow.instance.createError("Nieprawidłowy kolor");
                return;
            }
            

            transform.GetComponent<Image>().color = color;
            return;
        } 
        Vector2 point;
        if (desktopRect == null) return;
        var isvalid = RectTransformUtility.ScreenPointToLocalPointInRectangle(desktopRect, Input.mousePosition, Camera.main, out point);
        if (isvalid)
            MouseDragStartPos = (Vector3) point-transform.localPosition;
        focusObject();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (PhotoshopWindow.instance.mouseOnImage == this){
            PhotoshopWindow.instance.mouseOnImage = null;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        PhotoshopWindow.instance.mouseOnImage = this;
    }
}
