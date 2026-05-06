using UnityEngine;
using TMPro;
using System.Globalization;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        string wrapped = "{\"Items\":" + json + "}";
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(wrapped);
        return wrapper.Items;
    }

    [System.Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }
}
public class UIController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI killsText;
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] GameObject pauseMenuUI;

    [SerializeField] TextMeshProUGUI finalScoreText;
    [SerializeField] TextMeshProUGUI finalKillsText;
    [SerializeField] TextMeshProUGUI finalTimeText;
    [SerializeField] TextMeshProUGUI leaderboardText;

    [SerializeField] TMP_InputField nameInputField;
    private bool isPaused = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameOverPanel.SetActive(false);
        if (pauseMenuUI != null) pauseMenuUI.SetActive(false);

        GameManager.Instance.OnScoreChanged += UpdateScore;
        GameManager.Instance.OnGameOver += ShowGameOver;
        GameManager.Instance.OnKillsChanged += UpdateKills;

        UpdateScore(GameManager.Instance.Score);
        UpdateKills(GameManager.Instance.EnemiesKilled);
    }
    
    void UpdateScore(int score)
    {
        scoreText.text = $"Score: {score}";
    }

    void UpdateKills(int kills)
    {
        killsText.text = $"Kills: {kills}";
    }

    void ShowGameOver()
    {
        gameOverPanel.SetActive(true);

        finalScoreText.text = $"Final Score: {GameManager.Instance.Score}";
        finalKillsText.text = $"Final Kills: {GameManager.Instance.EnemiesKilled}";

        float timeSurvived = GameManager.Instance.RunTime;
        int minutes = Mathf.FloorToInt(timeSurvived / 60f);
        int seconds = Mathf.FloorToInt(timeSurvived % 60f);
        finalTimeText.text = $"Time: {minutes:00}:{seconds:00}";

        nameInputField.text = "";

        StartCoroutine(BackendAPI.GetLeaderboard(OnLeaderboardReceived));
    }

    public void OnSaveName(string ignored)
    {
        string actualName = nameInputField.text;
        Debug.Log("OnSaveName received: '" + actualName + "'");

        GameManager.Instance.SetDisplayName(actualName.Trim());
    }

    public void OnRetry()
    {
        // Reset game
        GameManager.Instance.ResetGame();
        gameOverPanel.SetActive(false);
    }

    private void OnLeaderboardReceived(string json)
    {
        RunData[] runs = JsonHelper.FromJson<RunData>(json);

        leaderboardText.text = "";

        for (int i = 0; i < runs.Length; i++)
        {
            string name = string.IsNullOrEmpty(runs[i].displayName) ? "Anonymous" : runs[i].displayName;
            int minutes = Mathf.FloorToInt(runs[i].timeSurvived / 60f);
            int seconds = Mathf.FloorToInt(runs[i].timeSurvived % 60f);

            string line = string.Format(
                "{0,2}. {1,-12} {2,5} ({3:00}:{4:00})",
                i+1,
                name,
                runs[i].score,
                minutes, seconds
                );

            leaderboardText.text += line + "\n";
        }
    }

    // Update is called once per frame
    void Update()
    {
        float timeSurvived = GameManager.Instance.RunTime;
        int minutes = Mathf.FloorToInt(timeSurvived / 60f);
        int seconds = Mathf.FloorToInt(timeSurvived % 60f);
        timeText.text = $"Time: {minutes:00}:{seconds:00}";

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (isPaused) Resume();
            else Pause();
        }
    }

    public void Resume()
    {
        if (pauseMenuUI != null) pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    void Pause()
    {
        if (pauseMenuUI != null) pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void QuitToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }


}
