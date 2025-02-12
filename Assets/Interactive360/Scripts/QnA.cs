using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QnA : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI TheQuestion;
    [SerializeField] TextMeshProUGUI FeedBack;


    public GameObject Buttons;
    int ChoiceMade;
    int UnitPoints;


    void Start()
    {
        FeedBack.text = " ";
    }


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

        StartCoroutine(SecondsWait());
        
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

        // imported.Assessment(activeScene);

        FeedBack.text = " "; 
        ChoiceMade = 0;
    }

    // Update is called once per frame
    void Work(int ChoiceMade)
    {
        // if(ChoiceMade >= 1)
        // {
            TheQuestion.text = " ";
            Buttons.SetActive(false);
            Scoring playerScore = FindObjectOfType<Scoring>();
        // }
        
    }

    IEnumerator SecondsWait()
    {
        yield return new WaitForSeconds(1);
        //ChoiceMade = 0;

        Buttons.SetActive(true);

        QtnReset();
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(2);
        
    }
}
