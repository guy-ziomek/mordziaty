using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class AppOnBar : MonoBehaviour
{
    private bool _isfocused;
    public bool isFocused {
        get {return _isfocused;}
        set {
            _isfocused = value;
            gameObject.transform.Find("focusedImage").GetComponent<Image>().enabled = _isfocused;
            }
    }
    public WindowUI windowReference;
    public AppObject appReference;
}
