using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.Networking;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    private void Awake()
    {
        if (Instance) Destroy(Instance);
        Instance = this;
    }

    private int score;

    [SerializeField]
    private TextMeshProUGUI StatusText, ScoreTextSubmission, ScoreText;
    [SerializeField]
    private TMP_InputField inputField;
    [SerializeField]
    private GameObject ScoreScrollView, ScoreContainer, feedbackContainer;
    [SerializeField]
    private PointsFeedback feedbackPrefab;
    [SerializeField]
    private ScoreText prefab;

    [SerializeField]
    private int bapPoints;
    [SerializeField]
    private int bapAndMovePoints;
    [SerializeField]
    private int hitObjectPoints;
    [SerializeField]
    private int smashObjectPoints;

    public GameObject ScoreWindow;
    private bool isGameOver;

    public void setGameOver()
    {
        isGameOver = true;
    }
    public void AddBapPoints()
    {
        AddScoreFeedback("Bap", bapPoints);
        AddScore(bapPoints);
    }
    public void AddBapAndMovePoints()
    {
        AddScoreFeedback("Trick Bap", bapPoints);
        AddScore(bapAndMovePoints);
    }
    public void AddHitObjectPoints()
    {
        AddScoreFeedback("Nudge", hitObjectPoints);
        AddScore(hitObjectPoints);
    }
    public void AddSmashObjectPoints()
    {
        AddScoreFeedback("Smash", smashObjectPoints);
        AddScore(smashObjectPoints);
    }
    public void AddScoreFeedback(string name, int value)
    {
        if (isGameOver) return;
        PointsFeedback newEntry = Instantiate(feedbackPrefab, feedbackContainer.transform);
        newEntry.nameText.text = name;
        newEntry.valueText.text = $"+{value}";
    }
    public void AddScore(int amount)
    {
        if (isGameOver) return;
        score += amount;
        ScoreText.text = $"Score: {score}";
        ScoreTextSubmission.text = $"Score: {score}";
    }
    public void ResetScore()
    {
        score = 0;
        ScoreText.text = $"Score: {score}";
        ScoreTextSubmission.text = $"Score: {score}";
    }
    public void Submit()
    {
        StartCoroutine(SubmitScore());
    }
    public void Fetch()
    {
        StartCoroutine(FetchScore());
    }
    private IEnumerator SubmitScore()
    {
        StatusText.text = "Uploading...";
        string playerName = inputField.text.Replace(" ", "%20");
        string gameName = Application.productName.Replace(" ", "%20");
        //string url = UnityWebRequest.EscapeURL($);
        using (UnityWebRequest request = new UnityWebRequest($"https://us-central1-highscore-manager.cloudfunctions.net/scores/postScore?game={gameName}&playerName={playerName}&score={score}"))
        {
            request.method = "POST";
            request.downloadHandler = new DownloadHandlerBuffer();
            yield return request.SendWebRequest();
            switch (request.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(": Error: " + request.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(": HTTP Error: " + request.error);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log("Received: " + request.downloadHandler.text);
                    break;
            }
        }
        yield return FetchScore();

    }
    private IEnumerator FetchScore()
    {
        StatusText.text = "Fetching Scores...";
        string gameName = Application.productName.Replace(" ", "%20");
        using (UnityWebRequest request = new UnityWebRequest($"https://us-central1-highscore-manager.cloudfunctions.net/scores/fetchScores?game={gameName}"))
        {
            request.method = "GET";
            request.downloadHandler = new DownloadHandlerBuffer();
            yield return request.SendWebRequest();
            switch (request.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(": Error: " + request.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(": HTTP Error: " + request.error);
                    break;
                case UnityWebRequest.Result.Success:
                    string data = $"{{\"scores\": {request.downloadHandler.text}}}";
                    Debug.Log(data);
                    HighScores recievedScores = JsonUtility.FromJson<HighScores>(data);
                    StatusText.gameObject.SetActive(false);
                    ScoreScrollView.SetActive(true);
                    PriorityQueue<ScoreData, int> sortedScores = recievedScores.ToPriorityQueue();
                    int index = 1;
                    while (sortedScores.Count > 0)
                    {
                        ScoreText text = Instantiate(prefab, ScoreContainer.transform);
                        ScoreData scoreData = sortedScores.Pop();
                        text.Index.text = $"{index}:";
                        text.Name.text = scoreData.playerName;
                        text.Score.text = scoreData.score.ToString();
                        index++;
                    }
                    break;
            }
        }
    }
}
[System.Serializable]
public class HighScores
{
    public List<ScoreData> scores;
    public PriorityQueue<ScoreData, int> ToPriorityQueue()
    {
        PriorityQueue<ScoreData, int> sortedScores = new PriorityQueue<ScoreData, int>(0);
        foreach (ScoreData score in scores)
        {
            sortedScores.Insert(score, int.MaxValue - score.score);
        }
        return sortedScores;
    }
}
[System.Serializable]
public class ScoreData
{
    public string id;
    public string playerName;
    public int score;
}