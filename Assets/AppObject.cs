using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

[CreateAssetMenu(fileName = "App", menuName = "ScriptableObjects/AppObject", order = 1)]
public class AppObject : ScriptableObject
{
    public string AppName;
    public WindowUI windowTemplate;
    public Sprite appImage;
}
