using TMPro;
using UnityEngine;
using System.Collections;


public class LeaderboardUI : MonoBehaviour
{
    public Transform contentParent;
    public GameObject entryPrefab;

    private void OnEnable()
    {
        StartCoroutine(BackendAPI.GetLeaderboard(OnLeaderboardReceived));
    }

    void OnLeaderboardReceived(string json)
    {
        RunData[] entries = JsonHelper.FromJson<RunData>(json);

        foreach (Transform child in contentParent) Destroy(child.gameObject);
        foreach (var e in entries)
        {
            GameObject row = Instantiate(entryPrefab, contentParent);
            TMP_Text[] texts = row.GetComponentsInChildren<TMP_Text>();

            int minutes = Mathf.FloorToInt(e.timeSurvived / 60f);
            int seconds = Mathf.FloorToInt(e.timeSurvived % 60f);

            texts[0].text = e.displayName;
            texts[1].text = e.score.ToString();
            texts[2].text = e.enemiesKilled.ToString();
            texts[3].text = $"{minutes:00}:{seconds:00}";
        }
    }
}
