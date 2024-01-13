using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TimelineScrob : MonoBehaviour, IPointerDownHandler
{
    RectTransform playhead;

    public void OnPointerDown(PointerEventData eventData)
    {
        Vector2 point;
        var rect = transform.GetComponent<RectTransform>();
        var isvalid = RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, Input.mousePosition, Camera.main, out point);
        if (!isvalid) return;
        var width = rect.rect.width;
        var pos = point + new Vector2(width/2,0);
        EditingWindow.instance.UpdateTime(pos.x/100); 
    }
    void Update()
    {
        
    }
}
