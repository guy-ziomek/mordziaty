using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct ExplorerFile {
    public string name;

}

[System.Serializable]
public struct VideoNames {
    public ExplorerFile[] videoNames;
}


public class YTWindow : WindowUI
{
    [SerializeField] TextAsset jsonFile;
    [SerializeField] AppObject fileExplorerApp;
    [SerializeField] Transform errorPopup;
    Transform rightside;

    [SerializeField] Slider progressBar;
    override protected void Start()
    {
        base.Start();
        rightside = transform.Find("Display/RightSide");  
    }

    private string getRandomFileName(){
        var rng = Random.Range(0,100);
        var dzien = Random.Range(1,29).ToString("00");
        var miesiac = Random.Range(1,13).ToString("00"); 
        var godzina = Random.Range(0,24).ToString("00");
        var minuta = Random.Range(0,60).ToString("00");
        
        
        switch(rng){
            case <=50:
                return $"{dzien}-{miesiac}-2024{godzina}{minuta}.mp4";
            case <= 75:
                var a = "";
                char[] b = {'a','s','d','f','w','e'};
                for (int i = 0; i < Random.Range(4,10); i++){
                    a+=b[Random.Range(0,b.Length)];
                }
                return $"{a}.mp4";
            case <= 95:
                ExplorerFile[] nazwyplikow = JsonUtility.FromJson<VideoNames>(jsonFile.text).videoNames;
                
                return nazwyplikow[Random.Range(0, nazwyplikow.Length)].name;
            case <= 98:
                return "asd";
            case <= 100:
                return "final.mp4";
        }
        return "a";
    }

    public void correctVideoFileChosenCallback(){
        //upload sequence
        
        GameManager.instance.gameState = GameState.Uploading;
        
    }

    public void openFileExplorer(){
        var theapp = desktopHandler.appsBarTransform.Find(fileExplorerApp.name);
        if (theapp != null){
            theapp.gameObject.GetComponent<AppOnBar>().windowReference.focusWindow();
            return;
        };

        FileExplorerWindow window = (FileExplorerWindow) DesktopHandler.instance.openApp(fileExplorerApp);
        
        List<string> files = new List<string>();

        for (int i = 0; i<40; i++){
            files.Add(getRandomFileName());
        }


        window.generateFiles(files, "prawdziwanazwa");
        window.callback = correctVideoFileChosenCallback;
    }
    public void uploadButtonPressed(){
        openFileExplorer();
    }

    [SerializeField] TMP_InputField titleBox;
    [SerializeField] TMP_InputField descBox;
    [SerializeField] TMP_InputField tagBox;
    
    bool thumbnailset = false;

    private bool Error(string message){
         var template = DesktopHandler.instance.createPopup(errorPopup);
            var reasonText = template.transform.Find("Reason").GetComponent<TMP_Text>();
            reasonText.text = message;
        return false;
    }

    private bool checkTitle(){
        var title = titleBox.text;
        if (title.Length < 5) return Error("Tytuł ma za mało znaków");

        return true;
    }

    private bool checkDescription(){
        var desc = descBox.text;
        if (desc.Length < 50) return Error("Opis musi mieć conajmniej 50 znaków!");
        return true;
    }

    private bool checkTags(){
        var tags = tagBox.text.Split(",");
        if (tags.Length <= 0){
            return Error("Nie podałeś tagów!");
        }
        if (tags.Length <= 3) return Error("Za mało tagów!");
        
        foreach(var tag in tags){
            if (tag.Length < 3) return Error("Tagi powinny sie składać z conajmniej 3 znaków");
        }
        
        return true;
    }

    public void publishButtonPressed(){
        if (!thumbnailset){
            Error("Brak miniaturki");
            return;
        }
        if (checkTitle() && checkDescription() && checkTags() && GameManager.instance.gameState == GameState.VideoUploaded){
            GameManager.instance.Win();
        }
    }

    public void SetThumbnailButtonPressed(){
        thumbnailset = true;
    }

    float progress = 0;
    float uploadTime = 50;
    void Update()
    {
        var showUploader = GameManager.instance.gameState == GameState.Uploading || GameManager.instance.gameState == GameState.VideoUploaded;
        var thumbnailFinished = GameManager.instance.gameState == GameState.ThumbnailFinished;
        rightside.Find("Upload").gameObject.SetActive(thumbnailFinished);
        rightside.Find("UploaderUI").gameObject.SetActive(showUploader);

        if (GameManager.instance.gameState == GameState.Uploading){
            progress+=Time.deltaTime;
            progressBar.value = progress/uploadTime;
            if (progress > uploadTime){
                GameManager.instance.gameState = GameState.VideoUploaded;
            }
        }
    }
}
