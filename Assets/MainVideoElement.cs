using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Video;


public class Cut {
    public float start;
    public float end;
    public float duration {
        get {return end-start;}
    }
    public Cut(float _start, float _end){
        start = _start;
        end = _end;
    }
}
public class MainVideoElement : MonoBehaviour
{
    public VideoClip videoClip;
    public double start = Mathf.Infinity;
    public double end = 0;

    public int cutsAmount;


    List<Cut> cuts = new List<Cut>();

    public void Init(VideoClip video, int cuts)
    {
        cutsAmount = cuts;
        videoClip = video;
        end = video.length;
        EditingWindow.instance.MainVidUpdate(this);
    }

    public double getTime(double time){
        for (int i = 0; i<cuts.Count; i++){
            var cut = cuts[i];
            if (time >= cut.start){
                time += cut.duration;
            }
        }
        return time;
    }

    public void AddCut(float startTime, float endTime){
        var change = Math.Abs(startTime-endTime);
        end -= change;
        startTime = (float) getTime(startTime);
        endTime = (float) getTime(endTime);
        var cut = new Cut(startTime,endTime);
        cuts.Add(cut);
        cuts.Sort((Cut obj, Cut obj2)=>{
            return obj.start.CompareTo(obj2.start);
        });
        var index = cuts.IndexOf(cut);
        for (int i = index+1; i < cuts.Count; i++){
            // dok przesuwanie dalszych nie dziala glitch ze usuwanie na poczatku cut z tylu a potem z przodu psuje ciecia
            var curCut = cuts[i];
            // curCut.start += change;
            // curCut.end += change;
        }
        EditingWindow.instance.MainVidUpdate(this,cuts.Count);
        
        // float minutes = Mathf.Floor((float) startTime / 60);
        // float seconds = Mathf.RoundToInt((float) startTime % 60);
        // float milsecs = Mathf.Round((float) (startTime%60*100)%100);
        // print($"{minutes.ToString("00")}:{seconds.ToString("00")}:{milsecs.ToString("00")}");
    }

    
}
