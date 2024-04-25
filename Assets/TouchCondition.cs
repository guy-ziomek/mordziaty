using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public class TouchCondition : ImageCondition
{
    
    public bool Overlaps(RectTransform a, RectTransform b) {

        Vector3[] cornersRect1 = new Vector3[4];
        a.GetLocalCorners(cornersRect1);
        Vector3[] cornersRect2 = new Vector3[4];
        b.GetLocalCorners(cornersRect2);

        var minCorner1 = a.localPosition + cornersRect1[0]; // bottom left
        var minCorner2 = b.localPosition + cornersRect2[0];

        var maxCorner1 = a.localPosition + cornersRect1[2]; // top right
        var maxCorner2 = b.localPosition + cornersRect2[2];

        if (minCorner1.x > maxCorner2.x) return false; //totally outside (right)
        if (minCorner1.y > maxCorner2.y) return false; //totally outside (top)

        if (maxCorner1.x < minCorner2.x) return false; //totally outside (left)
        if (maxCorner1.y < minCorner2.y) return false; //totally outside (bottom)
                
        return true;
    }

    [SerializeField] string touchObjectName;
    protected override bool _checkValid()
    {
        RectTransform obj = canvasRect.transform.Find(touchObjectName).GetComponent<RectTransform>();
        return Overlaps(rectTransform,obj);
        
    }

    

}

