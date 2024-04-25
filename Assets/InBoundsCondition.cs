using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class InBoundsCondition : ImageCondition
{
    [SerializeField] string restraintObjectName;

    protected override bool _checkValid()
    {
        RectTransform obj = canvasRect.transform.Find(restraintObjectName).GetComponent<RectTransform>();
        
        Vector2 anchoredPosition = rectTransform.anchoredPosition;
        
        var rightEdge = Mathf.Min(anchoredPosition.x+rectTransform.rect.width/2,canvasRect.rect.width/2-1);
        var leftEdge = Mathf.Max(anchoredPosition.x-rectTransform.rect.width/2,-canvasRect.rect.width/2+1);
        var topEdge = Mathf.Min(anchoredPosition.y+rectTransform.rect.height/2,canvasRect.rect.height/2-1);
        var downEdge = Mathf.Max(anchoredPosition.y-rectTransform.rect.height/2,-canvasRect.rect.height/2+1);


        if(rightEdge>obj.anchoredPosition.x+obj.rect.width*(1-obj.pivot.x)){
            return false;
        }
        if(leftEdge<obj.anchoredPosition.x-obj.rect.width*obj.pivot.x){
            return false;
        }
        if(topEdge>obj.anchoredPosition.y+obj.rect.height*(1-obj.pivot.y)){
            return false;
        }
        if(downEdge<obj.anchoredPosition.y-obj.rect.height*obj.pivot.y){
            return false;
        }
        
        
        // if(anchoredPosition.y+rectTransform.rect.height/2>canvasRect.rect.height/2){
        //     return false;
        // }
        // if(anchoredPosition.y-rectTransform.rect.height/2<-canvasRect.rect.height/2){
        //     return false;
        // }
        return true;
    }
}
