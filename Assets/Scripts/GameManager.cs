using Doozy.Engine.UI;
using System;
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
    // public UICanvas LoseScreen;
    // public UICanvas WinScreen;

    public bool SkipTitle;
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

    // Start is called before the first frame update

    // public bool IsAlarmActive => MajorHoles != 0 || MinorHoles != 0 || CurrentOxygen <= 25;

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        SceneManager.sceneLoaded += this.OnSceneLoaded;

        this.isPlaying = false;
        this.alreadyDead = false;

        if (this.SkipTitle && SceneManager.GetActiveScene().name != this.MainGame)
        {
            this.StartGame();
        }
        //else if(SceneManager.GetActiveScene().name != this.TitleScene)
        //{
        //    this.ReturnToTitle();
        //}
    }

    public void StartGame()
    {
        this.LoopCounter = 0;
        //this.CheckpointList = new bool[4];
        this.IsPlayerControllerEnabled = true;
        this.TimeRemaining = this.TimeToReset;
        this.TimerView.Show();
        // this.LoseScreen.gameObject.SetActive(false);
        // SceneManager.LoadScene(this.MainGame);
    }

    public void LoseGame()
    {
        this.isPlaying = false;
        // this.LoseScreen.gameObject.SetActive(true);
    }

    public void WinGame()
    {
        this.isPlaying = false;
        // this.WinScreen.gameObject.SetActive(true);
    }

    public void ReturnToTitle()
    {
        // this.LoseScreen.gameObject.SetActive(false);
        // this.WinScreen.gameObject.SetActive(false);
        this.IsPlayerControllerEnabled = false;
        // SceneManager.LoadScene(this.TitleScene);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void OnDisable()
    {
        // SceneManager.sceneLoaded -= this.OnSceneLoaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if(scene.name == this.MainGame)
        {
            this.ResetGameState();
        }
        else if(scene.name == this.TitleScene)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void ActivateCheckpoint(Checkpoints checkpoint)
    {
        this.CheckpointList[(int)checkpoint] = true;
    }

    public bool HasCheckpoint(Checkpoints checkpoint) => CheckpointList[(int) checkpoint];

    private void ResetGameState()
    {
        this.startTime = Time.deltaTime;
        this.isPlaying = true;
        this.TimeRemaining = this.TimeToReset;
        this.IsPlayerControllerEnabled = true;
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
        if (!this.NeverLose)
        {
            this.LoopCounter++;

            this.IsPlayerControllerEnabled = false;
            this.alreadyDead = true;

            PlayerDeathLoop pdl = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDeathLoop>();
            pdl.KillPlayer();

            OnReset();

            Action action = () => { this.IsPlayerControllerEnabled = true; this.alreadyDead = false; this.TimeRemaining = this.TimeToReset; };

            StartCoroutine(pdl.RespawnPlayer(action));
        }
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
