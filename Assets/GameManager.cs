using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState{
    Base = 0,
    Rendering = 1,
    VideoRendered = 2,
    ThumbnailFinished = 3,
    Uploading = 4,

    VideoUploaded = 5,

    VideoPublished = 6,

    End = 7
}

public class GameManager : MonoBehaviour
{

    public GameState gameState;
    public float renderTime = 2;
    public static GameManager instance;

    public float secondsSinceStart = 0;
    public int hour = 18;

    [SerializeField] private TMP_Text timeText;
    [SerializeField] private TMP_Text weekText;
    [SerializeField] private TMP_Text psychaText; 

    private EnemyScript[] enemies;

    public void onVideoRender(){
        gameState = GameState.VideoRendered;
    }
    void Start()
    {
        instance = this;
        enemies = GameObject.Find("Enemies").GetComponentsInChildren<EnemyScript>();
    }

    public void Win(){
        gameState = GameState.VideoPublished;
        print("you win");
    }

    // Update is called once per frame

    private void TimeOut(){
        print("nie zdazyles na czas...");
        gameState = GameState.End;
        SceneManager.LoadScene("GameOver");
    }
    public void GameOver(string killreason){
        print(killreason);
        gameState = GameState.End;
        SceneManager.LoadScene("GameOver");
    }

    void UpdateAI(){
        foreach(EnemyScript enemy in enemies){
            enemy.moveTime+=Time.deltaTime;
            if (enemy.moveTime > enemy.secondsToTryMove){
                enemy.moveTime = 0;
                enemy.TryMove();
            }
        }
    }

    void Update()
    {
        if (gameState == GameState.End) return;


        UpdateAI();

        secondsSinceStart+=Time.deltaTime;
        hour = 18+(int)math.floor(secondsSinceStart/60);
        timeText.text = $"{hour}:00";
        if(hour==24){
            TimeOut();
        }
    }
}
