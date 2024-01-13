using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DeleteFragmentScript : MonoBehaviour, IPointerDownHandler
{
    public Action onPressEvent;
    public void OnPointerDown(PointerEventData eventData)
    {
        if (onPressEvent!=null){
            onPressEvent();
            Destroy(gameObject);
        }
    }
}
