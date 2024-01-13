using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseControl : MonoBehaviour
{
    [SerializeField] private Texture2D defaultCursor, blockCursor, cutCursor;
    public static MouseControl instance;

    private void Awake(){
        if (instance == null){
            instance = this;
            DontDestroyOnLoad(gameObject);
        }else{
            Destroy(gameObject);
        }
    }

    public void CutCursor(){
        Cursor.SetCursor(cutCursor, Vector2.zero, CursorMode.Auto);
    }

    public void Default(){
        Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
    }
    
    public void BlockCursor(){
        Cursor.SetCursor(blockCursor, Vector2.zero, CursorMode.Auto);
    }
}

