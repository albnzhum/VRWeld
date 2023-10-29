using System;
using System.Collections;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public class ScoreManager : MonoBehaviour
{
    private string baseUrl = "http://xn----7sbpbfclakh1al9a7fxc.xn--p1ai:8000/";
    private string scoreEndpoint = "sessions/create";

    public IEnumerator CreateSession(
        DateTime dateStart, TimeSpan duration, 
        int score, int maxScore, string desciptionEvalitonReason, string accessToken, string refreshToken)
    {
        string url = baseUrl + scoreEndpoint;

        Session newSession = new Session(
            PlayerPrefs.GetInt("ModuleId"), refreshToken, dateStart, duration, score, maxScore, true, 5, desciptionEvalitonReason, null);
        string jsonData = JsonConvert.SerializeObject(newSession);
        Debug.Log(jsonData);

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        
        request.SetRequestHeader("accept", "application/json");
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + accessToken);
        
        yield return request.SendWebRequest();
        string responseText = request.downloadHandler.text;
        print(responseText);

        if (request.responseCode == 200)
        {
            Debug.Log("success");
        }
        else
        {
            Debug.LogError(request.responseCode);
        }
    }

}

[System.Serializable]
public class Session
{
    public int module_id;
    public string user_id;
    public DateTime date;
    public TimeSpan duration;
    public int score;
    public int maxScore;
    public bool is_successful;
    public int mark;
    public string description_evaluation_reason;
    public string url_recording_file;

    public Session(int moduleID, string userId,
        DateTime date, TimeSpan duration, 
        int score, int maxScore, bool isSuccessful, int mark, string descriptionEvaluationReason, string url)
    {
        module_id = moduleID;
        user_id = userId;
        this.date = date;
        this.duration = duration;
        this.score = score;
        this.maxScore = maxScore;
        is_successful = isSuccessful;
        this.mark = mark;
        description_evaluation_reason = descriptionEvaluationReason;
        url_recording_file = url;
    }
}
