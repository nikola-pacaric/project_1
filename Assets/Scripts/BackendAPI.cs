using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public class RunData
{
    public string runId;
    public string displayName;
    public int score;
    public int enemiesKilled;
    public float timeSurvived;
}

public static class BackendAPI
{
    private const string BASE_URL = "https://project1-backend-6el6.onrender.com";

    public static IEnumerator SendRunData(string runId, string displayName, int score, int enemiesKilled, float runTime)
    {
        string url = BASE_URL + "/runs";

        RunData runData = new RunData
        {
            runId = runId,
            displayName = displayName,
            score = score,
            enemiesKilled = enemiesKilled,
            timeSurvived = runTime
        };

        string json = JsonUtility.ToJson(runData);

        using (UnityWebRequest req = new UnityWebRequest(url, "POST"))
        {
            req.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(json));
            req.downloadHandler = new DownloadHandlerBuffer();
            req.SetRequestHeader("Content-Type", "application/json");

            yield return req.SendWebRequest();

            if (req.result != UnityWebRequest.Result.Success)
                Debug.LogError("Error saving run: " + req.error);
            else
                Debug.Log("Run saved: " + req.downloadHandler.text);
        }
    }

    public static IEnumerator UpdateRunName(string runId, string displayName)
    {
        string url = BASE_URL + "/runs/" + runId;

        RunData updateData = new RunData
        {
            runId = runId,
            displayName = displayName
        };

        string json = JsonUtility.ToJson(updateData);

        using (UnityWebRequest req = new UnityWebRequest(url, "PUT"))
        {
            req.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(json));
            req.downloadHandler = new DownloadHandlerBuffer();
            req.SetRequestHeader("Content-Type", "application/json");

            yield return req.SendWebRequest();

            if (req.result != UnityWebRequest.Result.Success)
                Debug.LogError("Error updating name: " + req.error);
            else
                Debug.Log("Name updated: " + req.downloadHandler.text);
        }
    }

    public static IEnumerator GetLeaderboard(System.Action<string> callback)
    {
        string url = BASE_URL + "/leaderboard";

        using (UnityWebRequest req = UnityWebRequest.Get(url))
        {
            yield return req.SendWebRequest();

            if (req.result != UnityWebRequest.Result.Success)
                Debug.LogError("Error fetching leaderboard: " + req.error);
            else
                callback(req.downloadHandler.text);
        }
    }
}