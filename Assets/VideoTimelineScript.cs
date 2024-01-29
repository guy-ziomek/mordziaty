using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VideoTimelineScript : MonoBehaviour, IDropHandler, IPointerMoveHandler, IPointerExitHandler, IPointerEnterHandler
{
    DesktopHandler desktopHandler;
    RectTransform track1;
    RectTransform track2;
    RectTransform mainVideoPrefab;
    RectTransform memPrefab;
    DragableVideoScript _drag;
    DragableVideoScript ghostDrag{
        get{
            if (_drag == null && ghost !=null){
                _drag = ghost.GetComponent<DragableVideoScript>();
            }
            return _drag;
        }
    }

    CanvasGroup _ghostCanvasGroup;
    CanvasGroup ghostCanvasGroup{
        get{
            if (_ghostCanvasGroup == null && ghost !=null){
                _ghostCanvasGroup = ghost.transform.Find("RedLight").GetComponent<CanvasGroup>();
            }
            return _ghostCanvasGroup;
        }
    }


    RectTransform ghost;

    private void clear(){
        ghost = null;
        _ghostCanvasGroup = null;
        _drag = null;
    }



    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;
        MouseControl.instance.Default();
        if (desktopHandler.currentDrag != null && ghost!=null){
            var curDrag = desktopHandler.currentDrag;
            var dragg = curDrag.GetComponent<DragableVideoFile>();
            if (!curDrag.isValid) return;
            if (dragg == null){
                if (!ghostDrag.canBePlaced()) {
                    Destroy(ghost.gameObject);
                    clear();
                    return;
                };
                ghostDrag.videoClip = curDrag.vid;
                EditingWindow.instance.clipsMade+=1;
                ghostDrag.updateTimes();
            }
            if (dragg != null){
                ghost.GetComponent<MainVideoScript>().generateCuts(dragg.cuts);
                ghost.GetComponent<MainVideoElement>().Init(curDrag.vid, dragg.cuts/2);
            }
            ghost.GetComponent<CanvasGroup>().alpha = 1;
            clear();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        MouseControl.instance.Default();
        if (desktopHandler.currentDrag && ghost!=null){
            Destroy(ghost.gameObject);
            clear();
        }
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        if (desktopHandler.currentDrag == null) return;
        var curDrag = desktopHandler.currentDrag;
            if (!curDrag.isValid) {
                MouseControl.instance.BlockCursor();
                return;
        }
        if (curDrag.GetComponent<DragableVideoFile>()){
            ghost = Instantiate<RectTransform>(mainVideoPrefab);
            ghost.SetParent(track1,false);
        }else{
            ghost = Instantiate<RectTransform>(memPrefab);
            ghost.Find("ImageThing").GetComponent<Image>().sprite = curDrag.transform.Find("Image").GetComponent<Image>().sprite;
            ghost.SetParent(track2,false);
        }
        ghost.anchoredPosition = Vector3.zero;
        ghost.GetComponent<CanvasGroup>().alpha = 0.5f;
        ghost.sizeDelta = new Vector2((float) curDrag.length*100, ghost.sizeDelta.y);

    }

    public void OnPointerMove(PointerEventData eventData)
    {
        if (desktopHandler.currentDrag == null) return;
        var curDrag = desktopHandler.currentDrag;
            if (!curDrag.isValid) {
                MouseControl.instance.BlockCursor();
                return;
            }
        if (!(curDrag.GetComponent<DragableVideoFile>())){
            Vector2 point;
            var isvalid = RectTransformUtility.ScreenPointToLocalPointInRectangle(track2.GetComponent<RectTransform>(), Input.mousePosition, Camera.main, out point);
            if (ghost==null) return;
            var roundedX = Mathf.Round(((Vector3)point).x/8)*8;
            ghost.localPosition = new Vector2(roundedX, ghost.localPosition.y);
            if (ghostDrag.canBePlaced()){
                ghostCanvasGroup.alpha = 0;
            }
            else{
                ghostCanvasGroup.alpha = 0.6f;
            }
        }
        MouseControl.instance.Default();
    }



    // Start is called before the first frame update
    void Start()
    {
        desktopHandler = GetComponentInParent<DesktopHandler>();
        track1 = transform.Find("Track1").GetComponent<RectTransform>();
        track2 = transform.Find("Track2").GetComponent<RectTransform>();
        mainVideoPrefab = Resources.Load<GameObject>("MainVideoPrefab").GetComponent<RectTransform>();
        memPrefab = Resources.Load<GameObject>("MemxDPrefab").GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1) && ghost != null){
            Destroy(ghost.gameObject);
            clear();
            MouseControl.instance.Default();     
        }
    }
}
