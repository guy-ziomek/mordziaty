using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InfoboxScript : MonoBehaviour
{
    public void SetDisplay(Transform template){
        var title = transform.Find("WinBar/title").GetComponent<TMP_Text>();
        title.text = template.name;
        foreach(Transform child in template){
            var clone = Instantiate<Transform>(child,transform,false);
            clone.name = child.name;
        }
    }
}
