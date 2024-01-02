using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesktopAppMaxOneScript : DesktopAppScript
{
    protected override void openApp()
    {
        var theapp = desktopHandler.appsBarTransform.Find(app.name);
        if (theapp != null){
            theapp.gameObject.GetComponent<AppOnBar>().windowReference.focusWindow();
            return;
        };
        base.openApp();
    }
}
