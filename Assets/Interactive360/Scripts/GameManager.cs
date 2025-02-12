using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;

//The Game Manager keeps track of which scenes to load, handles loading scenes, fading between scenes and also video playing/pausing

namespace Interactive360
{

    public class GameManager : MonoBehaviour
    {

        [SerializeField] TextMeshProUGUI TheQuestion;
        [SerializeField] TextMeshProUGUI FeedBack;
        [SerializeField] TextMeshProUGUI Answer;
        
        [SerializeField] TextMeshProUGUI Option1;
        [SerializeField] TextMeshProUGUI Option2;
        [SerializeField] TextMeshProUGUI Option3;
        [SerializeField] TextMeshProUGUI Option4;

        public static GameManager instance = null;

        Scene scene;
        VideoPlayer video;
        Animator anim;
        Image fadeImage;

        AsyncOperation operation;


        [Header("Scene Management")]
        public string[] scenesToLoad;
        public string activeScene;

        public int TheScene;

        [Space]
        [Header("UI Settings")]

        public bool useFade;
        public GameObject fadeOverlay;
        public GameObject ControlUI;
        public GameObject LoadingUI;

        [SerializeField] TextMeshProUGUI currentMinutes;
        [SerializeField] TextMeshProUGUI currentSeconds;
        [SerializeField] TextMeshProUGUI totalMinutes;
        [SerializeField] TextMeshProUGUI totalSeconds;
        // public Text currentMinutes;
        // public Text currentSeconds;
        // public Text totalMinutes;
        // public Text totalSeconds;

        public GameObject Words_Screen;
        public GameObject Timer_Screen;
        public GameObject Menu_Btn;

        [Space]
        [Header("Assessment Feature")]
        public GameObject Assessment_panel;

        [HideInInspector]
        public float QnId;
        public GameObject AnswerArea;

        public GameObject ConfirmScreen;
        public GameObject QuestionHolder;
        public GameObject QuizPanel;

        bool done;


        //make sure that we only have a single instance of the game manager
        void Awake()
        {
            if (instance == null)
            {
                DontDestroyOnLoad(gameObject);
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
        }

        
        //set the initial active scene
        void Start()
        {
            scene = SceneManager.GetActiveScene();
            activeScene = scene.buildIndex + " - " + scene.name;

            Menu_Btn.SetActive(false);

            Assessment_panel.SetActive(false);

            done = false;

        }

        void Update()
        {

            if(Timer_Screen.active){
                Words_Screen.SetActive(false);
            }

            
            if(video.isPlaying)
            {
                SetcurrentTimeUI();
                SetTotalTimeUI();

                Timer_Screen.SetActive(true);

                Menu_Btn.SetActive(false);
            }else{
                //Show the Menu Button
                Menu_Btn.SetActive(true);
            }

            Assessment(activeScene);
            
        }

        public void bringMenu()
        {
            if(Words_Screen.active){
                Words_Screen.SetActive(false);
                Timer_Screen.SetActive(true);
            }else{
                Words_Screen.SetActive(true);
                Timer_Screen.SetActive(false);
            }
            
        }

        //Select scene is called from either the menu manager or hotspot manager, and is used to load the desired scene
        public void SelectScene(string sceneToLoad)
        {
            //if we want to use the fading between scenes, start the coroutine here
            if (useFade)
            {
                StartCoroutine(FadeOutAndIn(sceneToLoad));
            }
            //if we dont want to use fading, just load the next scene
            else
            {
                SceneManager.LoadScene(sceneToLoad);
            }
            //set the active scene to the next scene
            activeScene = sceneToLoad;
        }

        IEnumerator FadeOutAndIn(string sceneToLoad)
        {
            //get references to animatior and image component 
            anim = fadeOverlay.GetComponent<Animator>();
            fadeImage = fadeOverlay.GetComponent<Image>();

            //turn control UI off and loading UI on
            ControlUI.SetActive(false);
            LoadingUI.SetActive(true);

            //set FadeOut to true on the animator so our image will fade out
            anim.SetBool("FadeOut", true);

            //wait until the fade image is entirely black (alpha=1) then load next scene
            yield return new WaitUntil(() => fadeImage.color.a == 1);
            SceneManager.LoadScene(sceneToLoad);
            Scene scene = SceneManager.GetSceneByName(sceneToLoad);
            Debug.Log("loading scene:" + scene.name);
            yield return new WaitUntil(() => scene.isLoaded);

            // grab video and wait until it is loaded and prepared before starting the fade out
            video = FindObjectOfType<VideoPlayer>();
            yield return new WaitUntil(() => video.isPrepared);

            //set FadeOUt to false on the animator so our image will fade back in 
            anim.SetBool("FadeOut", false);
            
            //wait until the fade image is completely transparent (alpha = 0) and then turn loading UI off and control UI back on
            yield return new WaitUntil(() => fadeImage.color.a == 0);
            LoadingUI.SetActive(false);
            
            //if we have not destroyed the control UI, set it to active
            if (ControlUI) 
            ControlUI.SetActive(true);

            

        }

        //Find the video in the scene and pause it
        public void PauseVideo()
        {
            if (!video)
            {
                video = FindObjectOfType<VideoPlayer>();
            }
            video.Pause();
        }

        //Find the video in the scene and play it
        public void PlayVideo()
        {
            if (!video)
            {
                video = FindObjectOfType<VideoPlayer>();
            }
            video.Play();
        }

        
        public string SetcurrentTimeUI()
        {
            string minutes = Mathf.Floor ((int)video.time / 60).ToString ("00");
            string seconds = ((int)video.time % 60).ToString ("00");

            currentMinutes.text = minutes;
            currentSeconds.text = seconds;
            // Debug.Log("Current time set");

            string time = minutes + seconds;
            return time;
        }

        public void SetTotalTimeUI()
        {
        string minutes = Mathf.Floor ((int)video.clip.length / 60).ToString ("00");
        string seconds = ((int)video.clip.length % 60).ToString ("00");

        totalMinutes.text = minutes;
        totalSeconds.text = seconds;
        // Debug.Log("Total time set");


        }

       public double CalculatePlayedFraction()
        {
            double fraction = (double)video.frame / (double)video.clip.frameCount;
            return fraction;
        }

        public void Assessment(string activeScene)
        {
            if(activeScene == "CLINICAL_CARE")
            {
                TheScene = 1;
            }
            if(activeScene == "DF_COVERALL")
            {
                TheScene = 2;
            }
            if(activeScene == "DF_GOWN")
            {
                TheScene = 3;
            }
            if(activeScene == "DN_COVERALL")
            {
                TheScene = 4;
            }
            if(activeScene == "DN_GOWN")
            {
                TheScene = 5;
            }
            if(activeScene == "DNDF_MASK")
            {
                TheScene = 6;
            }
            if(activeScene == "GLOVING_AND_DEGLOVING")
            {
                TheScene = 7;
            }
            if(activeScene == "HAND_RUB")
            {
                TheScene = 8;
            }
            if(activeScene == "HAND_WASH")
            {
                TheScene = 9;
            }

            string Min_Sec = SetcurrentTimeUI();

            switch(TheScene) 
                {
                case 1:
                
                if(Min_Sec == "0021")
                {
                    Ask(1, 1);
                }
                    
                    break;
                case 2:
                  if(Min_Sec == "0023")
                {
                    Ask(2, 1);
                }
                    break;
                case 3:
                    if(Min_Sec == "0011")
                {
                    Ask(3, 1);
                }
                    break;
                case 4:
                   if(Min_Sec == "0031")
                {
                    Ask(4, 1);
                }
                    break;
                case 5:
                    if(Min_Sec == "0027")
                {
                    Ask(5, 1);
                }
                    break;
                case 6:
                   if(Min_Sec == "0029")
                {
                    Ask(6, 1);
                }
                    break;
                case 7:
                    if(Min_Sec == "0020")
                {
                    Ask(7, 1);
                }
                    break;
                case 8:
                   if(Min_Sec == "0021")
                {
                    Ask(8, 1);
                }
                    break;
                case 9:
                    if(Min_Sec == "0023")
                {
                    Ask(9, 1);
                }
                    break;
                default:
                    // code block
                    break;
                }
        }


        
        public void Ask(int level, int qn)
        {
            if(level == 1)
            {
                if(qn==1)
                {
                    QnId = 1.1f;
                    TheQuestion.text = "Category: "+ activeScene + "\n" + "Question One of this category";
                    Answer.text = "Corona Virus";

                    Option1.text = "First";
                    Option2.text = "Second";
                    Option3.text = "Third";
                    Option4.text = "Fourth";

                    // Debug.Log("Total time set");
                }

                if(qn==2)
                {
                    QnId = 1.2f;
                    TheQuestion.text = "Category: "+ activeScene + "\n" +  "Question Two of this category";
                    Answer.text = "Corona Virus";
                    
                    Option1.text = "First";
                    Option2.text = "Second";
                    Option3.text = "Third";
                    Option4.text = "Fourth";
                }

                if(qn==3)
                {
                    QnId = 1.3f;
                    TheQuestion.text = "Category: "+ activeScene + "\n" +  "Question Three of this category";
                    Answer.text = "Corona Virus";
                    
                    Option1.text = "First";
                    Option2.text = "Second";
                    Option3.text = "Third";
                    Option4.text = "Fourth";
                }
            }


            if(level == 2)
            {
                if(qn==1)
                {
                    QnId = 2.1f;
                    TheQuestion.text = "Category: "+ activeScene + "\n" +  "Question One of this category";
                    Answer.text = "Corona Virus";

                    Option1.text = "First";
                    Option2.text = "Second";
                    Option3.text = "Third";
                    Option4.text = "Fourth";
                }

                if(qn==2)
                {
                    QnId = 2.2f;
                    TheQuestion.text = "Category: "+ activeScene + "\n" +  "Question Two of this category";
                    Answer.text = "Corona Virus";
                    
                    Option1.text = "First";
                    Option2.text = "Second";
                    Option3.text = "Third";
                    Option4.text = "Fourth";
                }

                if(qn==3)
                {
                    QnId = 2.3f;
                    TheQuestion.text = "Category: "+ activeScene + "\n" +  "Question Three of this category";
                    Answer.text = "Corona Virus";
                    
                    Option1.text = "First";
                    Option2.text = "Second";
                    Option3.text = "Third";
                    Option4.text = "Fourth";
                }
            }

            if(level == 3)
            {
                if(qn==1)
                {
                    QnId = 3.1f;
                    TheQuestion.text = "Category: "+ activeScene + "\n" +  "Question One of this category";
                    Answer.text = "Corona Virus";

                    Option1.text = "First";
                    Option2.text = "Second";
                    Option3.text = "Third";
                    Option4.text = "Fourth";
                }

                if(qn==2)
                {
                    QnId = 3.2f;
                    TheQuestion.text = "Category: "+ activeScene + "\n" +  "Question Two of this category";
                    Answer.text = "Corona Virus";
                    
                    Option1.text = "First";
                    Option2.text = "Second";
                    Option3.text = "Third";
                    Option4.text = "Fourth";
                }

                if(qn==3)
                {
                    QnId = 3.3f;
                    TheQuestion.text = "Category: "+ activeScene + "\n" +  "Question Three of this category";
                    Answer.text = "Corona Virus";
                    
                    Option1.text = "First";
                    Option2.text = "Second";
                    Option3.text = "Third";
                    Option4.text = "Fourth";
                }
            }

            if(level == 4)
            {
                if(qn==1)
                {
                    QnId = 4.1f;
                    TheQuestion.text = "Category: "+ activeScene + "\n" +  "Question One of this category";
                    Answer.text = "Corona Virus";

                    Option1.text = "First";
                    Option2.text = "Second";
                    Option3.text = "Third";
                    Option4.text = "Fourth";

                    // Debug.Log("Total time set");
                }

                if(qn==2)
                {
                    QnId = 4.2f;
                    TheQuestion.text = "Category: "+ activeScene + "\n" + "Question Two of this category";
                    Answer.text = "Corona Virus";
                    
                    Option1.text = "First";
                    Option2.text = "Second";
                    Option3.text = "Third";
                    Option4.text = "Fourth";
                }

                if(qn==3)
                {
                    QnId = 4.3f;
                    TheQuestion.text = "Category: "+ activeScene + "\n" + "Question Three of this category";
                    Answer.text = "Corona Virus";
                    
                    Option1.text = "First";
                    Option2.text = "Second";
                    Option3.text = "Third";
                    Option4.text = "Fourth";
                }
            }


            if(level == 5)
            {
                if(qn==1)
                {
                    QnId = 5.1f;
                    TheQuestion.text = "Category: "+ activeScene + "\n" + "Question One of this category";
                    Answer.text = "Corona Virus";

                    Option1.text = "First";
                    Option2.text = "Second";
                    Option3.text = "Third";
                    Option4.text = "Fourth";
                }

                if(qn==2)
                {
                    QnId = 5.2f;
                    TheQuestion.text = "Category: "+ activeScene + "\n" + "Question Two of this category";
                    Answer.text = "Corona Virus";
                    
                    Option1.text = "First";
                    Option2.text = "Second";
                    Option3.text = "Third";
                    Option4.text = "Fourth";
                }

                if(qn==3)
                {
                    QnId = 5.3f;
                    TheQuestion.text = "Category: "+ activeScene + "\n" + "Question Three of this category";
                    Answer.text = "Corona Virus";
                    
                    Option1.text = "First";
                    Option2.text = "Second";
                    Option3.text = "Third";
                    Option4.text = "Fourth";
                }
            }

            if(level == 6)
            {
                if(qn==1)
                {
                    QnId = 6.1f;
                    TheQuestion.text = "Category: "+ activeScene + "\n" + "Question One of this category";
                    Answer.text = "Corona Virus";

                    Option1.text = "First";
                    Option2.text = "Second";
                    Option3.text = "Third";
                    Option4.text = "Fourth";
                }

                if(qn==2)
                {
                    QnId = 6.2f;
                    TheQuestion.text = "Category: "+ activeScene + "\n" + "Question Two of this category";
                    Answer.text = "Corona Virus";
                    
                    Option1.text = "First";
                    Option2.text = "Second";
                    Option3.text = "Third";
                    Option4.text = "Fourth";
                }

                if(qn==3)
                {
                    QnId = 6.3f;
                    TheQuestion.text = "Category: "+ activeScene + "\n" + "Question Three of this category";
                    Answer.text = "Corona Virus";
                    
                    Option1.text = "First";
                    Option2.text = "Second";
                    Option3.text = "Third";
                    Option4.text = "Fourth";
                }
            }

            if(level == 7)
            {
                if(qn==1)
                {
                    QnId = 7.1f;
                    TheQuestion.text = "Category: "+ activeScene + "\n" + "Question One of this category";
                    Answer.text = "Corona Virus";

                    Option1.text = "First";
                    Option2.text = "Second";
                    Option3.text = "Third";
                    Option4.text = "Fourth";

                    // Debug.Log("Total time set");
                }

                if(qn==2)
                {
                    QnId = 7.2f;
                    TheQuestion.text = "Category: "+ activeScene + "\n" + "Question Two of this category";
                    Answer.text = "Corona Virus";
                    
                    Option1.text = "First";
                    Option2.text = "Second";
                    Option3.text = "Third";
                    Option4.text = "Fourth";
                }

                if(qn==3)
                {
                    QnId = 7.3f;
                    TheQuestion.text = "Category: "+ activeScene + "\n" + "Question Three of this category";
                    Answer.text = "Corona Virus";
                    
                    Option1.text = "First";
                    Option2.text = "Second";
                    Option3.text = "Third";
                    Option4.text = "Fourth";
                }
            }


            if(level == 8)
            {
                if(qn==1)
                {
                    QnId = 8.1f;
                    TheQuestion.text = "Category: "+ activeScene + "\n" + "Question One of this category";
                    Answer.text = "Corona Virus";

                    Option1.text = "First";
                    Option2.text = "Second";
                    Option3.text = "Third";
                    Option4.text = "Fourth";
                }

                if(qn==2)
                {
                    QnId = 8.2f;
                    TheQuestion.text = "Category: "+ activeScene + "\n" + "Question Two of this category";
                    Answer.text = "Corona Virus";
                    
                    Option1.text = "First";
                    Option2.text = "Second";
                    Option3.text = "Third";
                    Option4.text = "Fourth";
                }

                if(qn==3)
                {
                    QnId = 8.3f;
                    TheQuestion.text = "Category: "+ activeScene + "\n" + "Question Three of this category";
                    Answer.text = "Corona Virus";
                    
                    Option1.text = "First";
                    Option2.text = "Second";
                    Option3.text = "Third";
                    Option4.text = "Fourth";
                }
            }

            if(level == 9)
            {
                if(qn==1)
                {
                    QnId = 9.1f;
                    TheQuestion.text = "Category: "+ activeScene + "\n" +"Question One of this category";
                    Answer.text = "Corona Virus";

                    Option1.text = "First";
                    Option2.text = "Second";
                    Option3.text = "Third";
                    Option4.text = "Fourth";
                }

                if(qn==2)
                {
                    QnId = 9.2f;
                    TheQuestion.text = "Category: "+ activeScene + "\n" + "Question Two of this category";
                    Answer.text = "Corona Virus";
                    
                    Option1.text = "First";
                    Option2.text = "Second";
                    Option3.text = "Third";
                    Option4.text = "Fourth";
                }

                if(qn==3)
                {
                    QnId = 9.3f;
                    TheQuestion.text = "Category: "+ activeScene + "\n" + "Question Three of this category";
                    Answer.text = "Corona Virus";
                    
                    Option1.text = "First";
                    Option2.text = "Second";
                    Option3.text = "Third";
                    Option4.text = "Fourth";
                }
            }

            PauseVideo();
            Assessment_panel.SetActive(true);

            Timer_Screen.SetActive(false);

            // StartCoroutine(WaitForMySeconds());

            if(done)
            {
                Done();
            }
        } 

        IEnumerator WaitForMySeconds()
        {
            yield return new WaitForSeconds(5);
            done = false;
        }

        public void Done()
        {
            PlayVideo();
            AnswerArea.SetActive(false);
            FeedBack.text = " ";
            // QuizPanel.SetActive(true);
            QuestionHolder.SetActive(true);
            Buttons.SetActive(true);
            
            Assessment_panel.SetActive(false);
            


            Timer_Screen.SetActive(true);

            StartCoroutine(WaitForMySeconds());
        }



    public GameObject Buttons;
    int ChoiceMade;
    int UnitPoints;


    public void ChoiceOption1()
    {
        FeedBack.text = "Wrong";
        ChoiceMade = 1;
        UnitPoints = -10;

        Work(ChoiceMade);
        Scoring playerScore = FindObjectOfType<Scoring>();
        
        playerScore.AddPoints(UnitPoints);

        StartCoroutine(SecondsWait());
    }

    public void ChoiceOption2()
    {
        FeedBack.text = "Wrong";
        UnitPoints = -10;
        ChoiceMade = 2;

        Work(ChoiceMade);
        Scoring playerScore = FindObjectOfType<Scoring>();

        playerScore.AddPoints(UnitPoints);

        StartCoroutine(SecondsWait());
    }

    public void ChoiceOption3()
    {
        FeedBack.text = "Correct";
        ChoiceMade = 3;
        UnitPoints = 20;

        Work(ChoiceMade);
        Scoring playerScore = FindObjectOfType<Scoring>();

        playerScore.AddPoints(UnitPoints);
        Buttons.SetActive(false);
        QuestionHolder.SetActive(false);

        AnswerArea.SetActive(true);

        // StartCoroutine(WaitForMySeconds());
        
    }

    public void MakeDone()
    {
        done = true;
    }

    public void ChoiceOption4()
    {
        FeedBack.text = "Wrong";
        ChoiceMade = 4;
        UnitPoints = -10;

        Work(ChoiceMade);
        Scoring playerScore = FindObjectOfType<Scoring>();

        playerScore.AddPoints(UnitPoints);

        StartCoroutine(SecondsWait());
    }

    public void QtnReset()
    {
        //Functionality from the GameManager Script
        // imported = GameObject.FindGameObjectWithTag("TagA").GetComponent<GameManager>();

        // Assessment(activeScene);

        FeedBack.text = " "; 
        ChoiceMade = 0;
    }

    // Update is called once per frame
    void Work(int ChoiceMade)
    {
        if(ChoiceMade != 3)
        {
            // TheQuestion.text = " ";
            ConfirmScreen.SetActive(true);
            Buttons.SetActive(false);
            QuestionHolder.SetActive(false);
            // Scoring playerScore = FindObjectOfType<Scoring>();
        }else{
            AnswerArea.SetActive(true);
        }
        
    }

    IEnumerator SecondsWait()
    {
        yield return new WaitForSeconds(1);
        ChoiceMade = 0;

        ConfirmScreen.SetActive(true);

        QtnReset();
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(2);
        
    }

    public void RetryQn()
    {
        Buttons.SetActive(true);
        QuestionHolder.SetActive(true);
        ConfirmScreen.SetActive(false);
    }

    public void AcceptDefeat()
    {
        AnswerArea.SetActive(true);
        ConfirmScreen.SetActive(false);
    }


    }

}

