using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Video;



public class MainVideoScript : MonoBehaviour
{
    [SerializeField] private CutPlace cutPrefab;
    [SerializeField] private DeleteFragmentScript deletePrefab;
    public List<CutPlace> cutPlaces = new List<CutPlace>();

    public void CreateDeleteFragment(CutPlace cutPlace1, CutPlace cutPlace2){
        var rect1 = cutPlace1.GetComponent<RectTransform>();
        var rect2 = cutPlace2.GetComponent<RectTransform>();
        if (rect1.anchoredPosition.x>rect2.anchoredPosition.x) {
            var temprect = rect1;
            rect1 = rect2;
            rect2 = temprect;
        }
        var startPos = rect1.anchoredPosition.x;
        var endPos = rect2.anchoredPosition.x;

        var startWorldPos = rect1.position;
        var endWorldPos = rect2.position;

        var pos = (startPos+endPos)/2;
        var pref = Instantiate<DeleteFragmentScript>(deletePrefab);
        pref.transform.SetParent(transform,false);
        var prefrect = pref.GetComponent<RectTransform>();
        prefrect.anchoredPosition = new Vector2(pos, prefrect.position.y);
        prefrect.sizeDelta = new Vector2(Mathf.Abs(startPos-endPos),prefrect.sizeDelta.y);


        pref.onPressEvent = () =>{
            var pref = Instantiate<DeleteFragmentScript>(deletePrefab);
            pref.transform.SetParent(transform,false);
            var prefrect = pref.GetComponent<RectTransform>();
            prefrect.anchoredPosition = new Vector2(startPos,prefrect.position.y);
            prefrect.sizeDelta = new Vector2(2,prefrect.sizeDelta.y);

            var test = transform.GetComponent<RectTransform>();
            test.sizeDelta = new Vector2(test.sizeDelta.x-Mathf.Abs(startPos-endPos),test.sizeDelta.y);
            foreach(RectTransform child in test){
                if (child.anchoredPosition.x > endPos)
                    child.anchoredPosition -= new Vector2(Mathf.Abs(startPos-endPos),0)/2;
                else{
                    child.anchoredPosition += new Vector2(Mathf.Abs(startPos-endPos),0)/2; 
                }
            }
            MainVideoElement mainVideoElement = test.GetComponent<MainVideoElement>();
            var rect = transform.parent.GetComponent<RectTransform>();

            startPos = transform.parent.InverseTransformPoint(startWorldPos).x;
            endPos = transform.parent.InverseTransformPoint(endWorldPos).x;
            startPos += rect.rect.width/2;
            endPos += rect.rect.width/2;
            mainVideoElement.AddCut(startPos/100,endPos/100);
        };
    }

    bool checkDistance(RectTransform rect, float newPos, float distance){
        foreach(RectTransform r in rect.parent){
            if (r == rect) continue;
            var p = Mathf.Abs(newPos-r.anchoredPosition.x);
            if (p < distance) return false;
        }
        return true;
    }
    float FindNewPosX(float min, float max, RectTransform rect) {
        float newPos;
        var canPass = true;
        int maxC = 0;
        do {
            newPos = Random.Range(min, max);
            canPass = checkDistance(rect, newPos,25);
            maxC++;
        } while (!canPass && maxC < 200);
    return newPos;
} 
    public int? getCut(CutPlace cut){
        int i = 0;
        foreach (CutPlace cutPlace in cutPlaces){
            if (cutPlace == cut) return i;
            i++;
        }
        return null;
    }
    public void generateCuts(int amount){
        for (int i = 0; i<amount; i++){
            var pref = Instantiate<CutPlace>(cutPrefab,transform,false);
            var cutRectT = pref.GetComponent<RectTransform>();
            var RectTransform = GetComponent<RectTransform>();
            var wid = RectTransform.rect.width/2;
            var x = FindNewPosX(-wid+10,wid, cutRectT);
            cutRectT.anchoredPosition = new Vector2(x,pref.transform.localPosition.y);
            cutPlaces.Add(pref);
        }
        cutPlaces.Sort((CutPlace obj, CutPlace obj2)=>{
            var rect1 = obj.GetComponent<RectTransform>();
            var rect2 = obj2.GetComponent<RectTransform>();

            return rect1.anchoredPosition.x.CompareTo(rect2.anchoredPosition.x);
        });
        int v = 0;
        foreach(CutPlace cutpl in cutPlaces){
            v++;
        }
    }




}
