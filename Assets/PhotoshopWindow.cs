using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public enum PhotoshopMode{
    Move = 0,
    Color = 1
}

public class PhotoshopWindow : WindowUI
{
    [SerializeField] Transform popup;
    [SerializeField] Transform successPopup;
    public static PhotoshopWindow instance;
    public TMP_InputField hexColorInput;

    public Color currentColor;
    public PhotoshopMode Mode;

    [SerializeField] TMP_Text tooltip;
    RectTransform tooltipRect;
    public PhotoshopImage mouseOnImage;
    
    public void ChangeMode(int mode){
        Mode = (PhotoshopMode) Enum.GetValues(typeof(PhotoshopMode)).GetValue(mode);
    }
    private void Awake(){
        instance = this;
    }

    override protected void Start(){
        base.Start();
        if (GameManager.instance.gameState != GameState.VideoRendered){
            close();
        }
        tooltipRect = tooltip.GetComponent<RectTransform>();
    }

    public void createError(string _text){
        var infobox = DesktopHandler.instance.createPopup(popup);
        var text = infobox.transform.Find("Reason").GetComponent<TMP_Text>();
        text.text = _text;
    }

    bool CheckImages(){
        foreach(PhotoshopImage photoshopImage in transform.Find("Display/RightSide/Canvas").GetComponentsInChildren<PhotoshopImage>()){
            var condition = photoshopImage.checkConditions();
            if (condition){
                var _text = string.Format(condition.errorMessage,condition.getFormatArguments(photoshopImage.fileName));
                createError(_text);
                return false;
            }
        }
        return true;
    }

    public void Save(){
        if (!CheckImages()) return;

        var template = DesktopHandler.instance.createPopup(successPopup);
        GameManager.instance.gameState = GameState.ThumbnailFinished;
    }
    
    void Update()
    {
        if (GameManager.instance.gameState != GameState.VideoRendered){
            close();
        }

        if (mouseOnImage){
            tooltip.text = mouseOnImage.fileName;
            Vector2 point;
            var isvalid = RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform) transform.Find("Display/RightSide/Canvas"), Input.mousePosition, Camera.main, out point);
            if (!isvalid) return;
            tooltipRect.localPosition = point;
            tooltip.gameObject.SetActive(true);
        }else{
            tooltip.gameObject.SetActive(false);
        }
    }
}
