using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Video;

public class DragableFile : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler

{
    private Canvas canvas;
    
    public bool isValid = false;
    public VideoClip vid;
    public double length {
        get {
            if (vid != null){
                return vid.length;
            }
            return 1;
        }
    }
    private RectTransform prefab;
    private RectTransform clone;
    private RectTransform rectTransform;
    private Vector3 MouseDragStartPos;
    private RectTransform canvasRect;
    private DesktopHandler desktopHandler;

    private void Awake(){
        canvas = GetComponentInParent<Canvas>();
        rectTransform = GetComponent<RectTransform>();
        canvasRect = canvas.GetComponent<RectTransform>();
    }
    private void Start(){
        prefab = Resources.Load<GameObject>("FileDragPrefab").GetComponent<RectTransform>();
        desktopHandler = GetComponentInParent<DesktopHandler>();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
         if (eventData.button != PointerEventData.InputButton.Left) return; 
        Vector2 point;
        var isvalid = RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, Input.mousePosition, Camera.main, out point);
        if (isvalid)
            clone = Instantiate<RectTransform>(prefab);
            clone.SetParent(canvasRect,false);
            clone.anchoredPosition = point;
            clone.GetComponent<Image>().sprite = transform.Find("Image").GetComponent<Image>().sprite;
            MouseDragStartPos = (Vector3) point-clone.localPosition;
            desktopHandler.currentDrag = this;
    }

    public void OnDrag(PointerEventData eventData)
    {
         Vector2 point;
            var isvalid = RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, Input.mousePosition, Camera.main, out point);
            if (isvalid){
                clone.anchoredPosition = (Vector3) point-MouseDragStartPos;
            }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (clone == null) return;
        Destroy(clone.gameObject);
        desktopHandler.currentDrag = null;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        
    }

    

    // Update is called once per frame
    void Update()
    {
        
    }
}
