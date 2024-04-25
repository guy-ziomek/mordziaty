using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageCondition : MonoBehaviour
{
    protected RectTransform rectTransform;
    protected RectTransform canvasRect;

    [SerializeField] protected bool reverseCondition = false;  
    public string errorMessage = "Plik {0} musi nie wychodzić poza granice obrazu";

    public virtual string[] getFormatArguments(string fileName){
        return new string[]{fileName};
    }

    protected virtual bool _checkValid(){
        Vector2 anchoredPosition = rectTransform.anchoredPosition;
        if(anchoredPosition.x+rectTransform.rect.width/2>canvasRect.rect.width/2){
            return false;
        }
        if(anchoredPosition.x-rectTransform.rect.width/2<-canvasRect.rect.width/2){
            return false;
        }
        if(anchoredPosition.y+rectTransform.rect.height/2>canvasRect.rect.height/2){
            return false;
        }
        if(anchoredPosition.y-rectTransform.rect.height/2<-canvasRect.rect.height/2){
            return false;
        }
        return true;
    }

    public bool checkValid(){
        return reverseCondition? !_checkValid() : _checkValid();
    }



    protected virtual void Start(){
        rectTransform = (RectTransform) transform;
        canvasRect = (RectTransform) PhotoshopWindow.instance.transform.Find("Display/RightSide/Canvas");
    }
    void Update(){
        // Debug.Log(checkValid());
    }
}
