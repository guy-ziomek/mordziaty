using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorCondition : ImageCondition
{

    public ColorCondition(){
        errorMessage = "Plik {0} musi być w kolorze {1}";
    }

    public string colorRequired = "#FFFFFF";
    protected override bool _checkValid()
    {
         Color color;
        var success = ColorUtility.TryParseHtmlString(colorRequired, out color);
        return color==GetComponent<Image>().color;
    }
    public override string[] getFormatArguments(string fileName)
    {
        return new string[]{fileName,colorRequired};
    }
}
