using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RenderScript : MonoBehaviour
{
    [SerializeField] Transform successTransform;
    Slider slider;
    float renderProgress = 0;
    bool finished = false;
    void finishRender(){
        GameManager.instance.onVideoRender();
        DesktopHandler.instance.createPopup(successTransform);
        gameObject.GetComponentInParent<WindowUI>().close();
    }
    void Start(){
        slider = GetComponent<Slider>();
        GameManager.instance.gameState = GameState.Rendering;
    }
    void Update()
    {
        if (finished) return;
        renderProgress+=Mathf.Abs(Mathf.Sin(Time.time)*1.5f)*Time.deltaTime;
        var progress = Mathf.Clamp01(renderProgress/GameManager.instance.renderTime);
        slider.value = progress;
        if (renderProgress>GameManager.instance.renderTime){
            finished = true;
            finishRender();
        }
    }
}
