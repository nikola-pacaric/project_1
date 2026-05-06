using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject leaderboardPanel;

    public void PlayGame()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void ShowLeaderboard()
    {
        leaderboardPanel.SetActive(true);
    }

    public void HideLeaderboard()
    {
        leaderboardPanel.SetActive(false);
    }
}
