using Doozy.Engine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SingletonBehaviour<GameManager>
{
    public string TitleScene;
    public string MainGame;
    public UIView TimerView;
    public TMP_Text TimerText;

    public UIView WinContainer;
    public TMP_Text WinText;

    //public bool SkipTitle;
    public bool NeverLose;

    public bool IsPlayerControllerEnabled;

    public float TimeRemaining;
    public float TimeToReset;

    public delegate void ResetAction();
    public static event ResetAction OnReset;

    public bool[] CheckpointList = new bool[7];
    public int LoopCounter;

    private bool isPlaying;
    private float startTime;

    private bool alreadyDead;

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);

        this.isPlaying = false;
        this.alreadyDead = true;

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void StartGame()
    {
        this.alreadyDead = false;
        this.LoopCounter = 0;
        this.IsPlayerControllerEnabled = true;
        this.TimeRemaining = this.TimeToReset;
        this.TimerView.Show();
    }

    public void WinGame()
    {
        this.isPlaying = false;
        this.IsPlayerControllerEnabled = false;
        this.WinContainer.Show();
        this.WinText.text = $"You escaped in {this.LoopCounter} loops!";
    }

    public void ReturnToTitle()
    {
        this.WinContainer.Hide();
        this.IsPlayerControllerEnabled = false;
        SceneManager.LoadScene(this.TitleScene);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ActivateCheckpoint(Checkpoints checkpoint)
    {
        this.CheckpointList[(int)checkpoint] = true;
    }

    public bool HasCheckpoint(Checkpoints checkpoint) => CheckpointList[(int) checkpoint];

    private void ResetGameState()
    {
        this.CheckpointList = new bool[7];

        this.LoopCounter = 0;
        this.startTime = Time.deltaTime;
        this.isPlaying = true;
        this.TimeRemaining = this.TimeToReset;
        this.IsPlayerControllerEnabled = true;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (scene.name == "Main")
        {
            this.ResetGameState();
            this.StartGame();
        }
    }

    private void Update()
    {
        this.TimeRemaining -= Time.deltaTime;
        this.TimeRemaining = Mathf.Clamp(this.TimeRemaining, 0, this.TimeToReset);

        if(this.TimeRemaining > 0 && !this.alreadyDead)
        {
            float seconds = Mathf.FloorToInt(this.TimeRemaining % 60);
            float milliseconds = 100 - ((int)(Time.timeSinceLevelLoad * 100f) % 100);

            this.TimerText.text = String.Format("{0:00}:{1:00}", seconds, milliseconds);
        }
        else if (this.TimeRemaining <= 0 && !this.alreadyDead)
        {
            this.TimerText.text = "00:00";
            this.KillRespawnPlayer();
        }
    }

    public void KillRespawnPlayer()
    {
        if (!NeverLose)
        {
            LoopCounter++;
            IsPlayerControllerEnabled = false;
            alreadyDead = true;

            var pdl = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDeathLoop>();
            StartCoroutine(RespawnPlayer(pdl));
        }
    }

    private IEnumerator RespawnPlayer(PlayerDeathLoop loop)
    {
        loop.KillPlayer();

        OnReset();
        
        loop.RespawnPlayer();
        
        yield return new WaitForSeconds(2);
        
        IsPlayerControllerEnabled = true; 
        alreadyDead = false; 
        TimeRemaining = TimeToReset;
    }

    
}

public enum Checkpoints
{
    None,
    GunRoomComplete,
    Room1Complete,
    Room2Complete,
    Room34Complete,
    Room5Complete,
    RoomFinalComplete
}
