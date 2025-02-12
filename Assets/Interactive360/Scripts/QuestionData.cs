[System.Serializable]
public class Question
{
    public string id;
    public string questionText;
    public string[] options;
    public int correctAnswerIndex;
    public float videoTimestamp;  // When the question should appear
    public string feedback;       // Feedback for after answering
    public float timeLimit;       // Optional time limit in seconds
}

[CreateAssetMenu(fileName = "QuestionSet", menuName = "SomaVR/Question Set")]
public class QuestionSet : ScriptableObject
{
    public string moduleId;
    public string moduleName;
    public List<Question> questions;
} 