using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Video;

public class EditingWindow : WindowUI
{
    [SerializeField] TMP_Text currentTimeText;
    [SerializeField] TMP_Text vidlengthText;
    [SerializeField] RectTransform playHead;
    MainVideoElement mainVideoElement;
    public DragableVideoScript currentSecondVideo;
    public static EditingWindow instance;
    public double currentTime = 0;
    double endTime = 0;
    bool _isplaying = false;
    bool isPlaying {
        get {return _isplaying;}
        set {
            _isplaying = value;
            if (_isplaying){
                videoPlayer.Play();
                if (secondaryVideoPlayer.isActiveAndEnabled) secondaryVideoPlayer.Play();
            } else {
                videoPlayer.Pause();
                secondaryVideoPlayer.Pause();
            }
            }
        }
    public VideoPlayer videoPlayer;
    public VideoPlayer secondaryVideoPlayer;

    private void Awake(){
        instance = this;
    }

    private void Start(){
        videoPlayer.controlledAudioTrackCount = 1;
        secondaryVideoPlayer.controlledAudioTrackCount = 1;
        videoPlayer.SetDirectAudioVolume(0,0.25f);
        secondaryVideoPlayer.SetDirectAudioVolume(0,0.75f);
    }
    
    public void MainVidUpdate(MainVideoElement element){
        endTime = element.end;
        videoPlayer.Stop();
        secondaryVideoPlayer.Stop();
        mainVideoElement = element;
        videoPlayer.clip = element.videoClip;  
        float minutes = Mathf.Floor((float) endTime / 60);
        float seconds = Mathf.RoundToInt((float) endTime % 60);
        float milsecs = Mathf.Round((float) (endTime%60*100)%100);
        vidlengthText.text = $"{minutes.ToString("00")}:{seconds.ToString("00")}:{milsecs.ToString("00")}";
    }
    public void Play(){
        if (currentTime == endTime) {currentTime = 0;}
        isPlaying = !isPlaying;
        if (isPlaying) {
            // currentTime = 0;
        }
    }
    public void StopPlaying(){
        isPlaying = false;
    }

    public void UpdateTime(double time){
        if (mainVideoElement == null) return;
        currentTime = time;
        if (currentTime > endTime) currentTime = endTime;
        float minutes = Mathf.Floor((float) currentTime / 60);
        float seconds = Mathf.RoundToInt((float) currentTime % 60);
        float milsecs = Mathf.Round((float) (currentTime%60*100)%100);
        currentTimeText.text = $"{minutes.ToString("00")}:{seconds.ToString("00")}:{milsecs.ToString("00")}";
        playHead.localPosition = new Vector2((float)currentTime*100,playHead.localPosition.y);   
        videoPlayer.time = mainVideoElement.getTime(currentTime);
        // videoPlayer.Play();
        if (currentSecondVideo != null) {
            if (secondaryVideoPlayer.clip != currentSecondVideo.videoClip)
                secondaryVideoPlayer.clip = currentSecondVideo.videoClip;
            secondaryVideoPlayer.time = currentSecondVideo.getTime(currentTime);
            secondaryVideoPlayer.Play();
        }
        
    }
    

    void Update(){
        secondaryVideoPlayer.gameObject.SetActive(currentSecondVideo != null);
        videoPlayer.gameObject.SetActive(mainVideoElement != null);
        if (!isPlaying) {
            secondaryVideoPlayer.Pause();    
            return;
            }
        if (!videoPlayer.isPrepared){videoPlayer.Prepare(); return;}
        UpdateTime(currentTime + Time.deltaTime);
        playHead.localPosition = new Vector2((float)currentTime*100,playHead.localPosition.y);
        if (currentTime >= endTime) {
            currentTime = endTime;
            isPlaying = false;
            videoPlayer.Stop();
        }    
    }


}
