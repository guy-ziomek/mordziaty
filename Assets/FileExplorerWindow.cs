using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


// public struct ExplorerFile {
//     string name;

// }
public class FileExplorerWindow : WindowUI
{   
    public Action callback;
    [SerializeField] private GameObject fileTemplate;

    string correctName;
    GameObject _chosenFile;
    GameObject chosenFile {get{
        return _chosenFile;
    } set{
        if (_chosenFile)
            updateCanvasTransparency(_chosenFile,0);
        _chosenFile = value;
        gameObject.transform.Find("SelectText").GetComponent<TMP_Text>().text = _chosenFile.name;
        updateCanvasTransparency(_chosenFile,1);
    }}
    protected override void Start()
    {
        base.Start();
    }

    void updateCanvasTransparency(GameObject obj, float opacity){
        obj.transform.GetComponentInChildren<CanvasGroup>().alpha = opacity;
    }

    private static void Shuffle<T>(List<T> ts) {
        var count = ts.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i) {
            var r = UnityEngine.Random.Range(i, count);
            var tmp = ts[i];
            ts[i] = ts[r];
            ts[r] = tmp;
        }
    }

    public void pressSelect(){
        if (chosenFile == null || callback == null || chosenFile.name != correctName) {
            
            var infobox = desktopHandler.createPopup();
            var text = infobox.transform.Find("Reason").GetComponent<TMP_Text>();
            if (chosenFile == null){
                text.text = "Wybierz plik.";
                return;
            }
            if (chosenFile.name == "final.mp4"){
            text.text = "?";
            }else{
            text.text = "Niepoprawny plik.";
            }
            return;
        }
        callback();
        close();
    }
    public void generateFiles(List<string> files, string trueFile){
        correctName = trueFile;
        files.Add(trueFile);
        Shuffle<string>(files);
        foreach(var file in files){
            var clone = Instantiate<GameObject>(fileTemplate);
            clone.SetActive(true);
            clone.transform.SetParent(transform.Find("Pliki/Viewport/Content"),false);
            clone.name = file;
            clone.transform.Find("filetitle").GetComponent<TMP_Text>().text = file;
            float lastTimeClicked = 0;
            clone.GetComponent<Button>().onClick.AddListener(() =>{
                chosenFile = clone;
                float curTime = Time.time;
                if (curTime-lastTimeClicked <= 0.35f){
                    pressSelect();
                }
                lastTimeClicked = curTime;
            });
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
