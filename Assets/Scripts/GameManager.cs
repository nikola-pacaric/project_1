using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int Score { get; private set; }
    public float RunTime { get; private set; }
    private bool isTimerRunning;
    public int EnemiesKilled { get; private set; }
    public event Action<int> OnKillsChanged;
    public event Action<int> OnScoreChanged;
    public event Action OnGameOver;

    [SerializeField] AudioClip backgroundMusic;

    //Player identity
    public string RunId { get; private set; }
    public string DisplayName { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void AddScore(int amount)
    {
        Score += amount;
        OnScoreChanged?.Invoke(Score);
    }

    public void RegisterKill()
    {
        EnemiesKilled++;
        OnKillsChanged?.Invoke(EnemiesKilled);
    }

    public void StartTimer()
    {
        RunTime = 0f;
        isTimerRunning = true;
    }

    public void StopTimer()
    {
        isTimerRunning = false;
    }

    public void ResetGame()
    {
        //New Run UUID
        RunId = System.Guid.NewGuid().ToString();

        //Reset spawner
        FindAnyObjectByType<SurvivalSpawner>().ResetSpawner();

        //Reset score
        Score = 0; 
        OnScoreChanged?.Invoke(Score);

        //Reset kills
        EnemiesKilled = 0;
        OnKillsChanged?.Invoke(EnemiesKilled);

        //Reset timer
        StartTimer();

        //Reset time
        Time.timeScale = 1f;

        //Reset player
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {    
            player.transform.position = gameObject.transform.position;
            

            var upgrades = player.GetComponent<PlayerUpgrades>();
            if (upgrades != null)
            {
                upgrades.ResetStats();
            }
        }

        foreach (var e in GameObject.FindGameObjectsWithTag("Enemy")) Destroy(e);
        foreach (var d in GameObject.FindGameObjectsWithTag("Drop")) Destroy(d);
        
    }

    public void EndGame()
    {
        StartCoroutine(EndGameSequence()); 
    }

    private IEnumerator EndGameSequence()
    {
        CameraShake cameraShake = Camera.main.GetComponent<CameraShake>();
        if (cameraShake != null)
        {
            yield return StartCoroutine(cameraShake.Shake());
        }


        Time.timeScale = 0f;
        StopTimer();
        OnGameOver?.Invoke();

        Debug.Log("Saving run with ID: " + RunId + " | Score: " + Score);

        //Save a run to backend
        StartCoroutine(BackendAPI.SendRunData(
            RunId,
            DisplayName,
            Score,
            EnemiesKilled,
            RunTime
        ));
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SoundManager.Instance.PlayMusic(backgroundMusic);

        //First Run UUID
        RunId = System.Guid.NewGuid().ToString();
        StartTimer();
    }

    public void SetDisplayName(string name)
    {
        DisplayName = name;

        Debug.Log("Updating run with ID: " +RunId+ " | New name: " + DisplayName);
        StartCoroutine(BackendAPI.UpdateRunName(RunId, DisplayName));
    }

    // Update is called once per frame
    void Update()
    {
        if (isTimerRunning)
        {
            RunTime += Time.deltaTime;
        }
    }
}
