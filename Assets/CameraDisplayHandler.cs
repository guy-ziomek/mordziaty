using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class CameraButton {
    public Button button;
    private Camera _camera;
    private RenderTexture texture;
    [HideInInspector] public int index;
    [HideInInspector]
    public Camera camera{
        get {
            if (_camera == null){
                texture = Resources.Load<RenderTexture>("CameraTexture");
                _camera = GameObject.Find(cameraName).GetComponent<Camera>();
            }
            return _camera;
        }
    }
    public void setState(bool isActive){
        if (isActive){
            camera.targetTexture = texture;
            return;
        }
        camera.targetTexture = null;
    }
    public string cameraName;
}

public class CameraDisplayHandler : MonoBehaviour
{
    public CameraButton[] cameras;
    private CameraButton currentCamera;

    void changeCamera(CameraButton cam){
        if (currentCamera != null){
            currentCamera.setState(false);
        }
        currentCamera = cam;
        currentCamera.setState(true);
    }

    void Start(){
        int i = 0;
        foreach (var camera in cameras){
            camera.index = i;
            camera.setState(false);
            camera.button.onClick.AddListener(() =>{
                changeCamera(camera);
            });
            i++;
        }
        changeCamera(cameras[0]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
