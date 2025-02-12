using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Threading.Tasks;

public class APIService : MonoBehaviour
{
    private const string API_BASE_URL = "https://your-api-endpoint.com/"; // Replace with actual API endpoint
    
    public static APIService Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public async void SendAssessmentData(AssessmentData data)
    {
        string jsonData = JsonUtility.ToJson(data);
        StartCoroutine(SendRequest("assessment", jsonData));
    }

    public async void SendUserPerformanceData(PerformanceData data)
    {
        string jsonData = JsonUtility.ToJson(data);
        StartCoroutine(SendRequest("performance", jsonData));
    }

    private IEnumerator SendRequest(string endpoint, string jsonData)
    {
        using (UnityWebRequest request = new UnityWebRequest(API_BASE_URL + endpoint, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"API Error: {request.error}");
            }
        }
    }
} 