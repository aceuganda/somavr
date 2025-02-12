using System.Collections;
using UnityEngine;
using UnityEngine.Video;

public class QuestionManager : MonoBehaviour
{
    [SerializeField] private QuestionSet questionSet;
    [SerializeField] private GameObject questionPanelPrefab;
    [SerializeField] private Transform canvasTransform;
    
    private VideoPlayer videoPlayer;
    private List<Question> activeQuestions = new List<Question>();
    private GameObject currentQuestionPanel;

    private void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        LoadQuestions();
    }

    private void LoadQuestions()
    {
        if (questionSet != null)
        {
            activeQuestions = new List<Question>(questionSet.questions);
            activeQuestions.Sort((q1, q2) => q1.videoTimestamp.CompareTo(q2.videoTimestamp));
        }
    }

    private void Update()
    {
        if (videoPlayer != null && activeQuestions.Count > 0)
        {
            CheckForQuestions();
        }
    }

    private void CheckForQuestions()
    {
        float currentTime = (float)videoPlayer.time;
        
        if (activeQuestions.Count > 0 && 
            currentTime >= activeQuestions[0].videoTimestamp)
        {
            ShowQuestion(activeQuestions[0]);
            activeQuestions.RemoveAt(0);
            videoPlayer.Pause();
        }
    }

    private void ShowQuestion(Question question)
    {
        if (currentQuestionPanel != null)
        {
            Destroy(currentQuestionPanel);
        }

        currentQuestionPanel = Instantiate(questionPanelPrefab, canvasTransform);
        var questionUI = currentQuestionPanel.GetComponent<QuestionUI>();
        questionUI.SetupQuestion(question, OnQuestionAnswered);
    }

    private void OnQuestionAnswered(Question question, int answerIndex, bool isCorrect)
    {
        // Record response and send to API
        RecordResponse(question.id, question.options[answerIndex], isCorrect);
        
        // Show feedback
        StartCoroutine(ShowFeedbackAndContinue(question, isCorrect));
    }

    private IEnumerator ShowFeedbackAndContinue(Question question, bool isCorrect)
    {
        // Show feedback for 3 seconds
        yield return new WaitForSeconds(3f);
        
        // Remove question panel
        if (currentQuestionPanel != null)
        {
            Destroy(currentQuestionPanel);
        }
        
        // Resume video
        videoPlayer.Play();
    }

    [System.Serializable]
    public class AssessmentData
    {
        public string sessionId;
        public string questionId;
        public float timeStamp;
        public string userResponse;
        public float responseTime;
        public bool isCorrect;
        public int attemptNumber;
        public string videoTimestamp;
    }

    [System.Serializable]
    public class PerformanceData
    {
        public string sessionId;
        public float totalTimeSpent;
        public int totalQuestionsAttempted;
        public int correctAnswers;
        public float averageResponseTime;
        public string moduleId;
        public List<string> completedSections;
    }

    private List<AssessmentData> assessmentResults = new List<AssessmentData>();
    private PerformanceData currentPerformance;
    private float sessionStartTime;
    private float questionStartTime;

    private void Start()
    {
        sessionStartTime = Time.time;
        InitializePerformanceData();
    }

    private void InitializePerformanceData()
    {
        currentPerformance = new PerformanceData
        {
            sessionId = System.Guid.NewGuid().ToString(),
            totalTimeSpent = 0,
            totalQuestionsAttempted = 0,
            correctAnswers = 0,
            averageResponseTime = 0,
            completedSections = new List<string>()
        };
    }

    public void OnQuestionDisplayed(string questionId)
    {
        questionStartTime = Time.time;
    }

    public void RecordResponse(string questionId, string response, bool correct)
    {
        float responseTime = Time.time - questionStartTime;

        AssessmentData data = new AssessmentData
        {
            sessionId = currentPerformance.sessionId,
            questionId = questionId,
            timeStamp = Time.time,
            userResponse = response,
            responseTime = responseTime,
            isCorrect = correct,
            attemptNumber = GetAttemptNumber(questionId),
            videoTimestamp = GetCurrentVideoTime()
        };

        // Update performance metrics
        currentPerformance.totalQuestionsAttempted++;
        if (correct) currentPerformance.correctAnswers++;
        currentPerformance.averageResponseTime = UpdateAverageResponseTime(responseTime);
        currentPerformance.totalTimeSpent = Time.time - sessionStartTime;

        // Send data to API
        APIService.Instance.SendAssessmentData(data);
        assessmentResults.Add(data);

        // Save locally as backup
        SaveAssessmentData();
    }

    public void OnSectionCompleted(string sectionId)
    {
        if (!currentPerformance.completedSections.Contains(sectionId))
        {
            currentPerformance.completedSections.Add(sectionId);
            APIService.Instance.SendUserPerformanceData(currentPerformance);
        }
    }

    private float UpdateAverageResponseTime(float newResponseTime)
    {
        return ((currentPerformance.averageResponseTime * (currentPerformance.totalQuestionsAttempted - 1)) + newResponseTime) 
                / currentPerformance.totalQuestionsAttempted;
    }

    private int GetAttemptNumber(string questionId)
    {
        return assessmentResults.Count(x => x.questionId == questionId) + 1;
    }

    private string GetCurrentVideoTime()
    {
        // Return current video time in format "HH:mm:ss"
        return "00:00:00"; // Replace with actual video time
    }

    private void SaveAssessmentData()
    {
        string json = JsonUtility.ToJson(new { results = assessmentResults });
        string path = Path.Combine(Application.persistentDataPath, "assessment_results.json");
        File.WriteAllText(path, json);
    }

    private void OnDestroy()
    {
        // Send final performance data when session ends
        currentPerformance.totalTimeSpent = Time.time - sessionStartTime;
        APIService.Instance.SendUserPerformanceData(currentPerformance);
    }
}